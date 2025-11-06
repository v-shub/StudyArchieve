using Domain.Interfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Services.Tests
{
    public class AcademicYearServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IAcademicYearRepository> _mockAcademicYearRepository;
        private readonly AcademicYearService _academicYearService;

        public AcademicYearServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockAcademicYearRepository = new Mock<IAcademicYearRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.AcademicYear)
                .Returns(_mockAcademicYearRepository.Object);

            _academicYearService = new AcademicYearService(_mockRepositoryWrapper.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllAcademicYears()
        {
            // Arrange
            var expectedAcademicYears = new List<AcademicYear>
            {
                new AcademicYear { Id = 1, YearLabel = "2022-2023" },
                new AcademicYear { Id = 2, YearLabel = "2023-2024" }
            };

            _mockAcademicYearRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(expectedAcademicYears);

            // Act
            var result = await _academicYearService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedAcademicYears, result);
            _mockAcademicYearRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyList = new List<AcademicYear>();

            _mockAcademicYearRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _academicYearService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockAcademicYearRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            _mockAcademicYearRepository
                .Setup(x => x.FindAll())
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _academicYearService.GetAll());
        }

        [Fact]
        public async Task Create_ShouldCallRepositoryCreateAndSave()
        {
            // Arrange
            var academicYear = new AcademicYear { Id = 1, YearLabel = "2023-2024" };

            _mockAcademicYearRepository
                .Setup(x => x.Create(academicYear))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _academicYearService.Create(academicYear);

            // Assert
            _mockAcademicYearRepository.Verify(x => x.Create(academicYear), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _academicYearService.Create(null));

            _mockAcademicYearRepository.Verify(x => x.Create(It.IsAny<AcademicYear>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_WhenSaveFails_ShouldNotCallCreate()
        {
            // Arrange
            var academicYear = new AcademicYear { Id = 1, YearLabel = "2023-2024" };

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .ThrowsAsync(new Exception("Save failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _academicYearService.Create(academicYear));

            _mockAcademicYearRepository.Verify(x => x.Create(academicYear), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdateAndSave()
        {
            // Arrange
            var academicYear = new AcademicYear { Id = 1, YearLabel = "Updated Year" };

            _mockAcademicYearRepository
                .Setup(x => x.Update(academicYear))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _academicYearService.Update(academicYear);

            // Assert
            _mockAcademicYearRepository.Verify(x => x.Update(academicYear), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _academicYearService.Update(null));

            _mockAcademicYearRepository.Verify(x => x.Update(It.IsAny<AcademicYear>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldCallRepositoryDeleteAndSave()
        {
            // Arrange
            var academicYearId = 1;
            var academicYear = new AcademicYear { Id = academicYearId, YearLabel = "2023-2024" };
            var academicYears = new List<AcademicYear> { academicYear };

            _mockAcademicYearRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<AcademicYear, bool>>>()))
                .ReturnsAsync(academicYears);

            _mockAcademicYearRepository
                .Setup(x => x.Delete(academicYear))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _academicYearService.Delete(academicYearId);

            // Assert
            _mockAcademicYearRepository.Verify(
                x => x.FindByCondition(It.IsAny<Expression<Func<AcademicYear, bool>>>()),
                Times.Once);
            _mockAcademicYearRepository.Verify(x => x.Delete(academicYear), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var nonExistingId = 999;
            var emptyList = new List<AcademicYear>();

            _mockAcademicYearRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<AcademicYear, bool>>>()))
                .ReturnsAsync(emptyList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _academicYearService.Delete(nonExistingId));

            _mockAcademicYearRepository.Verify(x => x.Delete(It.IsAny<AcademicYear>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _academicYearService.Delete(invalidId));

            _mockAcademicYearRepository.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<AcademicYear, bool>>>()), Times.Never);
            _mockAcademicYearRepository.Verify(x => x.Delete(It.IsAny<AcademicYear>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenMultipleRecordsFound_ShouldDeleteFirstOne()
        {
            // Arrange
            var academicYearId = 1;
            var academicYears = new List<AcademicYear>
            {
                new AcademicYear { Id = academicYearId, YearLabel = "First" },
                new AcademicYear { Id = academicYearId, YearLabel = "Second" } // Same ID - unusual but possible
            };

            _mockAcademicYearRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<AcademicYear, bool>>>()))
                .ReturnsAsync(academicYears);

            _mockAcademicYearRepository
                .Setup(x => x.Delete(It.IsAny<AcademicYear>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _academicYearService.Delete(academicYearId);

            // Assert
            _mockAcademicYearRepository.Verify(x => x.Delete(academicYears.First()), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public void Constructor_WithNullRepositoryWrapper_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AcademicYearService(null));
        }

        [Fact]
        public void Constructor_WithValidRepositoryWrapper_ShouldInitializeService()
        {
            // Arrange
            var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            // Act
            var service = new AcademicYearService(mockRepositoryWrapper.Object);

            // Assert
            Assert.NotNull(service);
        }
    }
}