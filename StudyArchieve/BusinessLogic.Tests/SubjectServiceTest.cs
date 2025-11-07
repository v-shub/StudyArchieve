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
    public class SubjectServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ISubjectRepository> _mockSubjectRepository;
        private readonly SubjectService _subjectService;

        public SubjectServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockSubjectRepository = new Mock<ISubjectRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.Subject)
                .Returns(_mockSubjectRepository.Object);

            _subjectService = new SubjectService(_mockRepositoryWrapper.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllSubjects()
        {
            // Arrange
            var expectedSubjects = new List<Subject>
            {
                new Subject { Id = 1, Name = "Mathematics" },
                new Subject { Id = 2, Name = "Physics" }
            };

            _mockSubjectRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(expectedSubjects);

            // Act
            var result = await _subjectService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedSubjects, result);
            _mockSubjectRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyList = new List<Subject>();

            _mockSubjectRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _subjectService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockSubjectRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            _mockSubjectRepository
                .Setup(x => x.FindAll())
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _subjectService.GetAll());
        }

        [Fact]
        public async Task Create_ShouldCallRepositoryCreateAndSave()
        {
            // Arrange
            var subject = new Subject { Id = 1, Name = "Chemistry" };

            _mockSubjectRepository
                .Setup(x => x.Create(subject))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _subjectService.Create(subject);

            // Assert
            _mockSubjectRepository.Verify(x => x.Create(subject), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _subjectService.Create(null));

            _mockSubjectRepository.Verify(x => x.Create(It.IsAny<Subject>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_WhenSaveFails_ShouldNotCallCreate()
        {
            // Arrange
            var subject = new Subject { Id = 1, Name = "Chemistry" };

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .ThrowsAsync(new Exception("Save failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _subjectService.Create(subject));

            _mockSubjectRepository.Verify(x => x.Create(subject), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdateAndSave()
        {
            // Arrange
            var subject = new Subject { Id = 1, Name = "Updated Subject" };

            _mockSubjectRepository
                .Setup(x => x.Update(subject))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _subjectService.Update(subject);

            // Assert
            _mockSubjectRepository.Verify(x => x.Update(subject), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _subjectService.Update(null));

            _mockSubjectRepository.Verify(x => x.Update(It.IsAny<Subject>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldCallRepositoryDeleteAndSave()
        {
            // Arrange
            var subjectId = 1;
            var subject = new Subject { Id = subjectId, Name = "Mathematics" };
            var subjects = new List<Subject> { subject };

            _mockSubjectRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Subject, bool>>>()))
                .ReturnsAsync(subjects);

            _mockSubjectRepository
                .Setup(x => x.Delete(subject))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _subjectService.Delete(subjectId);

            // Assert
            _mockSubjectRepository.Verify(
                x => x.FindByCondition(It.IsAny<Expression<Func<Subject, bool>>>()),
                Times.Once);
            _mockSubjectRepository.Verify(x => x.Delete(subject), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var nonExistingId = 999;
            var emptyList = new List<Subject>();

            _mockSubjectRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Subject, bool>>>()))
                .ReturnsAsync(emptyList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _subjectService.Delete(nonExistingId));

            _mockSubjectRepository.Verify(x => x.Delete(It.IsAny<Subject>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _subjectService.Delete(invalidId));

            _mockSubjectRepository.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Subject, bool>>>()), Times.Never);
            _mockSubjectRepository.Verify(x => x.Delete(It.IsAny<Subject>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenMultipleRecordsFound_ShouldDeleteFirstOne()
        {
            // Arrange
            var subjectId = 1;
            var subjects = new List<Subject>
            {
                new Subject { Id = subjectId, Name = "First" },
                new Subject { Id = subjectId, Name = "Second" }
            };

            _mockSubjectRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Subject, bool>>>()))
                .ReturnsAsync(subjects);

            _mockSubjectRepository
                .Setup(x => x.Delete(It.IsAny<Subject>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _subjectService.Delete(subjectId);

            // Assert
            _mockSubjectRepository.Verify(x => x.Delete(subjects.First()), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public void Constructor_WithNullRepositoryWrapper_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SubjectService(null));
        }

        [Fact]
        public void Constructor_WithValidRepositoryWrapper_ShouldInitializeService()
        {
            // Arrange
            var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            // Act
            var service = new SubjectService(mockRepositoryWrapper.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task Delete_WhenFindByConditionReturnsNull_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var subjectId = 1;
            List<Subject> nullList = null;

            _mockSubjectRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Subject, bool>>>()))
                .ReturnsAsync(nullList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _subjectService.Delete(subjectId));

            _mockSubjectRepository.Verify(x => x.Delete(It.IsAny<Subject>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }
    }
}