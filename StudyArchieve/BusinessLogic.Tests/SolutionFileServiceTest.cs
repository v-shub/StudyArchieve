using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Services.Tests
{
    public class SolutionFileServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IBackblazeService> _mockBackblazeService;
        private readonly Mock<ILogger<SolutionFileService>> _mockLogger;
        private readonly Mock<ISolutionFileRepository> _mockSolutionFileRepository;
        private readonly SolutionFileService _solutionFileService;

        public SolutionFileServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockBackblazeService = new Mock<IBackblazeService>();
            _mockLogger = new Mock<ILogger<SolutionFileService>>();
            _mockSolutionFileRepository = new Mock<ISolutionFileRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.SolutionFile)
                .Returns(_mockSolutionFileRepository.Object);

            _solutionFileService = new SolutionFileService(
                _mockRepositoryWrapper.Object,
                _mockBackblazeService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task UploadFileAsync_WithValidFile_ShouldUploadAndCreateRecord()
        {
            // Arrange
            var solutionId = 1;
            var fileMock = new Mock<IFormFile>();
            var fileName = "test.pdf";
            var fileContent = "test content";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));

            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.ContentType).Returns("application/pdf");
            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

            var uploadResult = new FileUploadResult
            {
                FileKey = "solutionFiles/guid_test.pdf",
                FileUrl = "https://bucket.s3.url/solutionFiles/guid_test.pdf",
                FileSize = ms.Length
            };

            _mockBackblazeService
                .Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>(), "solutionFiles"))
                .ReturnsAsync(uploadResult);

            _mockSolutionFileRepository
                .Setup(x => x.Create(It.IsAny<SolutionFile>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _solutionFileService.UploadFileAsync(solutionId, fileMock.Object);

            // Assert
            Assert.True(result > 0);
            _mockBackblazeService.Verify(x => x.UploadFileAsync(fileMock.Object, "solutionFiles"), Times.Once);
            _mockSolutionFileRepository.Verify(x => x.Create(It.Is<SolutionFile>(sf =>
                sf.SolutionId == solutionId &&
                sf.FileName == fileName &&
                sf.FilePath == uploadResult.FileKey)), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UploadFileAsync_WhenBackblazeFails_ShouldThrowException()
        {
            // Arrange
            var solutionId = 1;
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes("test content"));

            fileMock.Setup(f => f.FileName).Returns("test.pdf");
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

            _mockBackblazeService
                .Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>(), "solutionFiles"))
                .ThrowsAsync(new Exception("Upload failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _solutionFileService.UploadFileAsync(solutionId, fileMock.Object));

            _mockSolutionFileRepository.Verify(x => x.Create(It.IsAny<SolutionFile>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task UploadFileAsync_WithNullFile_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _solutionFileService.UploadFileAsync(1, null));
        }

        [Fact]
        public async Task GetFilesBySolutionIdAsync_ShouldReturnFileInfosWithUrls()
        {
            // Arrange
            var solutionId = 1;
            var solutionFiles = new List<SolutionFile>
            {
                new SolutionFile { Id = 1, SolutionId = solutionId, FileName = "file1.pdf", FilePath = "path1" },
                new SolutionFile { Id = 2, SolutionId = solutionId, FileName = "file2.pdf", FilePath = "path2" }
            };

            _mockSolutionFileRepository
                .Setup(x => x.GetBySolutionIdAsync(solutionId))
                .ReturnsAsync(solutionFiles);

            _mockBackblazeService
                .Setup(x => x.GetFileUrlAsync("path1"))
                .ReturnsAsync("https://url1.com");
            _mockBackblazeService
                .Setup(x => x.GetFileUrlAsync("path2"))
                .ReturnsAsync("https://url2.com");

            // Act
            var result = await _solutionFileService.GetFilesBySolutionIdAsync(solutionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("file1.pdf", result[0].FileName);
            Assert.Equal("https://url1.com", result[0].FileUrl);
            Assert.Equal("file2.pdf", result[1].FileName);
            Assert.Equal("https://url2.com", result[1].FileUrl);
        }

        [Fact]
        public async Task GetFilesBySolutionIdAsync_WhenNoFiles_ShouldReturnEmptyList()
        {
            // Arrange
            var solutionId = 1;
            var emptyList = new List<SolutionFile>();

            _mockSolutionFileRepository
                .Setup(x => x.GetBySolutionIdAsync(solutionId))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _solutionFileService.GetFilesBySolutionIdAsync(solutionId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockBackblazeService.Verify(x => x.GetFileUrlAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetFileByIdAsync_WithValidId_ShouldReturnFileInfo()
        {
            // Arrange
            var fileId = 1;
            var solutionFile = new SolutionFile { Id = fileId, SolutionId = 1, FileName = "test.pdf", FilePath = "filePath" };

            _mockSolutionFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(solutionFile);

            _mockBackblazeService
                .Setup(x => x.GetFileUrlAsync("filePath"))
                .ReturnsAsync("https://url.com");

            // Act
            var result = await _solutionFileService.GetFileByIdAsync(fileId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fileId, result.Id);
            Assert.Equal("test.pdf", result.FileName);
            Assert.Equal("https://url.com", result.FileUrl);
        }

        [Fact]
        public async Task GetFileByIdAsync_WithNonExistingId_ShouldReturnNull()
        {
            // Arrange
            var fileId = 999;

            _mockSolutionFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync((SolutionFile)null);

            // Act
            var result = await _solutionFileService.GetFileByIdAsync(fileId);

            // Assert
            Assert.Null(result);
            _mockBackblazeService.Verify(x => x.GetFileUrlAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteFileAsync_WithValidId_ShouldDeleteFromStorageAndDatabase()
        {
            // Arrange
            var fileId = 1;
            var solutionFile = new SolutionFile { Id = fileId, FilePath = "filePath" };

            _mockSolutionFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(solutionFile);

            _mockBackblazeService
                .Setup(x => x.DeleteFileAsync("filePath"))
                .ReturnsAsync(true);

            _mockSolutionFileRepository
                .Setup(x => x.Delete(solutionFile))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _solutionFileService.DeleteFileAsync(fileId);

            // Assert
            Assert.True(result);
            _mockBackblazeService.Verify(x => x.DeleteFileAsync("filePath"), Times.Once);
            _mockSolutionFileRepository.Verify(x => x.Delete(solutionFile), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task DeleteFileAsync_WithNonExistingId_ShouldReturnFalse()
        {
            // Arrange
            var fileId = 999;

            _mockSolutionFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync((SolutionFile)null);

            // Act
            var result = await _solutionFileService.DeleteFileAsync(fileId);

            // Assert
            Assert.False(result);
            _mockBackblazeService.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
            _mockSolutionFileRepository.Verify(x => x.Delete(It.IsAny<SolutionFile>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task DeleteFileAsync_WhenBackblazeDeleteFails_ShouldStillDeleteFromDatabase()
        {
            // Arrange
            var fileId = 1;
            var solutionFile = new SolutionFile { Id = fileId, FilePath = "filePath" };

            _mockSolutionFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(solutionFile);

            _mockBackblazeService
                .Setup(x => x.DeleteFileAsync("filePath"))
                .ReturnsAsync(false); // Backblaze delete fails

            _mockSolutionFileRepository
                .Setup(x => x.Delete(solutionFile))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _solutionFileService.DeleteFileAsync(fileId);

            // Assert
            Assert.True(result); // Still returns true even if Backblaze fails
            _mockBackblazeService.Verify(x => x.DeleteFileAsync("filePath"), Times.Once);
            _mockSolutionFileRepository.Verify(x => x.Delete(solutionFile), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task DeleteFileAsync_WhenDatabaseSaveFails_ShouldReturnFalse()
        {
            // Arrange
            var fileId = 1;
            var solutionFile = new SolutionFile { Id = fileId, FilePath = "filePath" };

            _mockSolutionFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(solutionFile);

            _mockBackblazeService
                .Setup(x => x.DeleteFileAsync("filePath"))
                .ReturnsAsync(true);

            _mockSolutionFileRepository
                .Setup(x => x.Delete(solutionFile))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _solutionFileService.DeleteFileAsync(fileId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DownloadFileAsync_WithValidId_ShouldReturnFileDownloadResult()
        {
            // Arrange
            var fileId = 1;
            var solutionFile = new SolutionFile { Id = fileId, FileName = "test.pdf", FilePath = "filePath" };
            var downloadResult = new FileDownloadResult
            {
                Content = new MemoryStream(Encoding.UTF8.GetBytes("content")),
                ContentType = "application/pdf",
                FileName = "original.pdf"
            };

            _mockSolutionFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(solutionFile);

            _mockBackblazeService
                .Setup(x => x.DownloadFileAsync("filePath"))
                .ReturnsAsync(downloadResult);

            // Act
            var result = await _solutionFileService.DownloadFileAsync(fileId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(downloadResult.Content, result.Content);
            Assert.Equal(downloadResult.ContentType, result.ContentType);
            Assert.Equal("test.pdf", result.FileName); // Should use database filename, not original
        }

        [Fact]
        public async Task DownloadFileAsync_WithNonExistingId_ShouldThrowFileNotFoundException()
        {
            // Arrange
            var fileId = 999;

            _mockSolutionFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync((SolutionFile)null);

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => _solutionFileService.DownloadFileAsync(fileId));

            _mockBackblazeService.Verify(x => x.DownloadFileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DownloadFileAsync_WhenBackblazeFails_ShouldPropagateException()
        {
            // Arrange
            var fileId = 1;
            var solutionFile = new SolutionFile { Id = fileId, FilePath = "filePath" };

            _mockSolutionFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(solutionFile);

            _mockBackblazeService
                .Setup(x => x.DownloadFileAsync("filePath"))
                .ThrowsAsync(new Exception("Download failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _solutionFileService.DownloadFileAsync(fileId));
        }

        [Fact]
        public void Constructor_WithNullParameters_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repositoryWrapper = new Mock<IRepositoryWrapper>();
            var backblazeService = new Mock<IBackblazeService>();
            var logger = new Mock<ILogger<SolutionFileService>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SolutionFileService(null, backblazeService.Object, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new SolutionFileService(repositoryWrapper.Object, null, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new SolutionFileService(repositoryWrapper.Object, backblazeService.Object, null));
        }
    }
}