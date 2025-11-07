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
    public class TaskTypeServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ITaskTypeRepository> _mockTaskTypeRepository;
        private readonly TaskTypeService _taskTypeService;

        public TaskTypeServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockTaskTypeRepository = new Mock<ITaskTypeRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.TaskType)
                .Returns(_mockTaskTypeRepository.Object);

            _taskTypeService = new TaskTypeService(_mockRepositoryWrapper.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllTaskTypes()
        {
            // Arrange
            var expectedTaskTypes = new List<TaskType>
            {
                new TaskType { Id = 1, Name = "Homework" },
                new TaskType { Id = 2, Name = "Exam" },
                new TaskType { Id = 3, Name = "Quiz" }
            };

            _mockTaskTypeRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(expectedTaskTypes);

            // Act
            var result = await _taskTypeService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedTaskTypes, result);
            _mockTaskTypeRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyList = new List<TaskType>();

            _mockTaskTypeRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _taskTypeService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockTaskTypeRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            _mockTaskTypeRepository
                .Setup(x => x.FindAll())
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _taskTypeService.GetAll());
        }

        [Fact]
        public async Task Create_ShouldCallRepositoryCreateAndSave()
        {
            // Arrange
            var taskType = new TaskType { Id = 1, Name = "Project" };

            _mockTaskTypeRepository
                .Setup(x => x.Create(taskType))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _taskTypeService.Create(taskType);

            // Assert
            _mockTaskTypeRepository.Verify(x => x.Create(taskType), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _taskTypeService.Create(null));

            _mockTaskTypeRepository.Verify(x => x.Create(It.IsAny<TaskType>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_WhenSaveFails_ShouldPropagateException()
        {
            // Arrange
            var taskType = new TaskType { Id = 1, Name = "Project" };

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .ThrowsAsync(new Exception("Save failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _taskTypeService.Create(taskType));

            _mockTaskTypeRepository.Verify(x => x.Create(taskType), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdateAndSave()
        {
            // Arrange
            var taskType = new TaskType { Id = 1, Name = "Updated Task Type" };

            _mockTaskTypeRepository
                .Setup(x => x.Update(taskType))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _taskTypeService.Update(taskType);

            // Assert
            _mockTaskTypeRepository.Verify(x => x.Update(taskType), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _taskTypeService.Update(null));

            _mockTaskTypeRepository.Verify(x => x.Update(It.IsAny<TaskType>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldCallRepositoryDeleteAndSave()
        {
            // Arrange
            var taskTypeId = 1;
            var taskType = new TaskType { Id = taskTypeId, Name = "Homework" };
            var taskTypes = new List<TaskType> { taskType };

            _mockTaskTypeRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<TaskType, bool>>>()))
                .ReturnsAsync(taskTypes);

            _mockTaskTypeRepository
                .Setup(x => x.Delete(taskType))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _taskTypeService.Delete(taskTypeId);

            // Assert
            _mockTaskTypeRepository.Verify(
                x => x.FindByCondition(It.IsAny<Expression<Func<TaskType, bool>>>()),
                Times.Once);
            _mockTaskTypeRepository.Verify(x => x.Delete(taskType), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var nonExistingId = 999;
            var emptyList = new List<TaskType>();

            _mockTaskTypeRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<TaskType, bool>>>()))
                .ReturnsAsync(emptyList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _taskTypeService.Delete(nonExistingId));

            _mockTaskTypeRepository.Verify(x => x.Delete(It.IsAny<TaskType>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _taskTypeService.Delete(invalidId));

            _mockTaskTypeRepository.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<TaskType, bool>>>()), Times.Never);
            _mockTaskTypeRepository.Verify(x => x.Delete(It.IsAny<TaskType>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenMultipleRecordsFound_ShouldDeleteFirstOne()
        {
            // Arrange
            var taskTypeId = 1;
            var taskTypes = new List<TaskType>
            {
                new TaskType { Id = taskTypeId, Name = "First" },
                new TaskType { Id = taskTypeId, Name = "Second" }
            };

            _mockTaskTypeRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<TaskType, bool>>>()))
                .ReturnsAsync(taskTypes);

            _mockTaskTypeRepository
                .Setup(x => x.Delete(It.IsAny<TaskType>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _taskTypeService.Delete(taskTypeId);

            // Assert
            _mockTaskTypeRepository.Verify(x => x.Delete(taskTypes.First()), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public void Constructor_WithNullRepositoryWrapper_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TaskTypeService(null));
        }

        [Fact]
        public void Constructor_WithValidRepositoryWrapper_ShouldInitializeService()
        {
            // Arrange
            var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            // Act
            var service = new TaskTypeService(mockRepositoryWrapper.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task Delete_WhenFindByConditionReturnsNull_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var taskTypeId = 1;
            List<TaskType> nullList = null;

            _mockTaskTypeRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<TaskType, bool>>>()))
                .ReturnsAsync(nullList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _taskTypeService.Delete(taskTypeId));

            _mockTaskTypeRepository.Verify(x => x.Delete(It.IsAny<TaskType>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }
    }
}