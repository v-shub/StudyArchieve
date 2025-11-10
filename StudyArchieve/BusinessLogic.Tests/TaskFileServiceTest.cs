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
    public class TaskFileServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IBackblazeService> _mockBackblazeService;
        private readonly Mock<ILogger<TaskFileService>> _mockLogger;
        private readonly Mock<ITaskFileRepository> _mockTaskFileRepository;
        private readonly TaskFileService _taskFileService;

        public TaskFileServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockBackblazeService = new Mock<IBackblazeService>();
            _mockLogger = new Mock<ILogger<TaskFileService>>();
            _mockTaskFileRepository = new Mock<ITaskFileRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.TaskFile)
                .Returns(_mockTaskFileRepository.Object);

            _taskFileService = new TaskFileService(
                _mockRepositoryWrapper.Object,
                _mockBackblazeService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task UploadFileAsync_WithValidFile_ShouldUploadAndCreateRecord()
        {
            // Arrange
            var taskId = 1;
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
                FileKey = "taskFiles/guid_test.pdf",
                FileUrl = "https://bucket.s3.url/taskFiles/guid_test.pdf",
                FileSize = ms.Length
            };

            var createdTaskFileId = 123;

            _mockBackblazeService
                .Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>(), "taskFiles"))
                .ReturnsAsync(uploadResult);

            _mockTaskFileRepository
                .Setup(x => x.Create(It.IsAny<TaskFile>()))
                .Callback<TaskFile>(tf => tf.Id = createdTaskFileId)
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskFileService.UploadFileAsync(taskId, fileMock.Object);

            // Assert
            Assert.Equal(createdTaskFileId, result);
            _mockBackblazeService.Verify(x => x.UploadFileAsync(fileMock.Object, "taskFiles"), Times.Once);
            _mockTaskFileRepository.Verify(x => x.Create(It.Is<TaskFile>(tf =>
                tf.TaskId == taskId &&
                tf.FileName == fileName &&
                tf.FilePath == uploadResult.FileKey)), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UploadFileAsync_WhenBackblazeFails_ShouldThrowException()
        {
            // Arrange
            var taskId = 1;
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes("test content"));

            fileMock.Setup(f => f.FileName).Returns("test.pdf");
            fileMock.Setup(f => f.Length).Returns(ms.Length);
            fileMock.Setup(f => f.OpenReadStream()).Returns(ms);

            _mockBackblazeService
                .Setup(x => x.UploadFileAsync(It.IsAny<IFormFile>(), "taskFiles"))
                .ThrowsAsync(new Exception("Upload failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _taskFileService.UploadFileAsync(taskId, fileMock.Object));

            _mockTaskFileRepository.Verify(x => x.Create(It.IsAny<TaskFile>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task UploadFileAsync_WithNullFile_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _taskFileService.UploadFileAsync(1, null));
        }

        [Fact]
        public async Task GetFilesByTaskIdAsync_ShouldReturnFileInfosWithUrls()
        {
            // Arrange
            var taskId = 1;
            var taskFiles = new List<TaskFile>
            {
                new TaskFile { Id = 1, TaskId = taskId, FileName = "file1.pdf", FilePath = "path1" },
                new TaskFile { Id = 2, TaskId = taskId, FileName = "file2.pdf", FilePath = "path2" }
            };

            _mockTaskFileRepository
                .Setup(x => x.GetByTaskIdAsync(taskId))
                .ReturnsAsync(taskFiles);

            _mockBackblazeService
                .Setup(x => x.GetFileUrlAsync("path1"))
                .ReturnsAsync("https://url1.com");
            _mockBackblazeService
                .Setup(x => x.GetFileUrlAsync("path2"))
                .ReturnsAsync("https://url2.com");

            // Act
            var result = await _taskFileService.GetFilesByTaskIdAsync(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("file1.pdf", result[0].FileName);
            Assert.Equal("https://url1.com", result[0].FileUrl);
            Assert.Equal("file2.pdf", result[1].FileName);
            Assert.Equal("https://url2.com", result[1].FileUrl);
        }

        [Fact]
        public async Task GetFilesByTaskIdAsync_WhenNoFiles_ShouldReturnEmptyList()
        {
            // Arrange
            var taskId = 1;
            var emptyList = new List<TaskFile>();

            _mockTaskFileRepository
                .Setup(x => x.GetByTaskIdAsync(taskId))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _taskFileService.GetFilesByTaskIdAsync(taskId);

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
            var taskFile = new TaskFile { Id = fileId, TaskId = 1, FileName = "test.pdf", FilePath = "filePath" };

            _mockTaskFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(taskFile);

            _mockBackblazeService
                .Setup(x => x.GetFileUrlAsync("filePath"))
                .ReturnsAsync("https://url.com");

            // Act
            var result = await _taskFileService.GetFileByIdAsync(fileId);

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

            _mockTaskFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync((TaskFile)null);

            // Act
            var result = await _taskFileService.GetFileByIdAsync(fileId);

            // Assert
            Assert.Null(result);
            _mockBackblazeService.Verify(x => x.GetFileUrlAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DeleteFileAsync_WithValidId_ShouldDeleteFromStorageAndDatabase()
        {
            // Arrange
            var fileId = 1;
            var taskFile = new TaskFile { Id = fileId, FilePath = "filePath" };

            _mockTaskFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(taskFile);

            _mockBackblazeService
                .Setup(x => x.DeleteFileAsync("filePath"))
                .ReturnsAsync(true);

            _mockTaskFileRepository
                .Setup(x => x.Delete(taskFile))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskFileService.DeleteFileAsync(fileId);

            // Assert
            Assert.True(result);
            _mockBackblazeService.Verify(x => x.DeleteFileAsync("filePath"), Times.Once);
            _mockTaskFileRepository.Verify(x => x.Delete(taskFile), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task DeleteFileAsync_WithNonExistingId_ShouldReturnFalse()
        {
            // Arrange
            var fileId = 999;

            _mockTaskFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync((TaskFile)null);

            // Act
            var result = await _taskFileService.DeleteFileAsync(fileId);

            // Assert
            Assert.False(result);
            _mockBackblazeService.Verify(x => x.DeleteFileAsync(It.IsAny<string>()), Times.Never);
            _mockTaskFileRepository.Verify(x => x.Delete(It.IsAny<TaskFile>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task DeleteFileAsync_WhenBackblazeDeleteFails_ShouldStillDeleteFromDatabase()
        {
            // Arrange
            var fileId = 1;
            var taskFile = new TaskFile { Id = fileId, FilePath = "filePath" };

            _mockTaskFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(taskFile);

            _mockBackblazeService
                .Setup(x => x.DeleteFileAsync("filePath"))
                .ReturnsAsync(false);

            _mockTaskFileRepository
                .Setup(x => x.Delete(taskFile))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _taskFileService.DeleteFileAsync(fileId);

            // Assert
            Assert.True(result);
            _mockBackblazeService.Verify(x => x.DeleteFileAsync("filePath"), Times.Once);
            _mockTaskFileRepository.Verify(x => x.Delete(taskFile), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task DeleteFileAsync_WhenDatabaseSaveFails_ShouldReturnFalse()
        {
            // Arrange
            var fileId = 1;
            var taskFile = new TaskFile { Id = fileId, FilePath = "filePath" };

            _mockTaskFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(taskFile);

            _mockBackblazeService
                .Setup(x => x.DeleteFileAsync("filePath"))
                .ReturnsAsync(true);

            _mockTaskFileRepository
                .Setup(x => x.Delete(taskFile))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _taskFileService.DeleteFileAsync(fileId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DownloadFileAsync_WithValidId_ShouldReturnFileDownloadResult()
        {
            // Arrange
            var fileId = 1;
            var taskFile = new TaskFile { Id = fileId, FileName = "test.pdf", FilePath = "filePath" };
            var downloadResult = new FileDownloadResult
            {
                Content = new MemoryStream(Encoding.UTF8.GetBytes("content")),
                ContentType = "application/pdf",
                FileName = "original.pdf"
            };

            _mockTaskFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(taskFile);

            _mockBackblazeService
                .Setup(x => x.DownloadFileAsync("filePath"))
                .ReturnsAsync(downloadResult);

            // Act
            var result = await _taskFileService.DownloadFileAsync(fileId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(downloadResult.Content, result.Content);
            Assert.Equal(downloadResult.ContentType, result.ContentType);
            Assert.Equal("test.pdf", result.FileName);
        }

        [Fact]
        public async Task DownloadFileAsync_WithNonExistingId_ShouldThrowFileNotFoundException()
        {
            // Arrange
            var fileId = 999;

            _mockTaskFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync((TaskFile)null);

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => _taskFileService.DownloadFileAsync(fileId));

            _mockBackblazeService.Verify(x => x.DownloadFileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task DownloadFileAsync_WhenBackblazeFails_ShouldPropagateException()
        {
            // Arrange
            var fileId = 1;
            var taskFile = new TaskFile { Id = fileId, FilePath = "filePath" };

            _mockTaskFileRepository
                .Setup(x => x.GetByIdAsync(fileId))
                .ReturnsAsync(taskFile);

            _mockBackblazeService
                .Setup(x => x.DownloadFileAsync("filePath"))
                .ThrowsAsync(new Exception("Download failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _taskFileService.DownloadFileAsync(fileId));
        }

        [Fact]
        public void Constructor_WithNullParameters_ShouldThrowArgumentNullException()
        {
            // Arrange
            var repositoryWrapper = new Mock<IRepositoryWrapper>();
            var backblazeService = new Mock<IBackblazeService>();
            var logger = new Mock<ILogger<TaskFileService>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TaskFileService(null, backblazeService.Object, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new TaskFileService(repositoryWrapper.Object, null, logger.Object));
            Assert.Throws<ArgumentNullException>(() => new TaskFileService(repositoryWrapper.Object, backblazeService.Object, null));
        }
    }
}