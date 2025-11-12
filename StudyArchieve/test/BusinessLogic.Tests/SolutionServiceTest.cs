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
    public class SolutionServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ISolutionRepository> _mockSolutionRepository;
        private readonly SolutionService _solutionService;

        public SolutionServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockSolutionRepository = new Mock<ISolutionRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.Solution)
                .Returns(_mockSolutionRepository.Object);

            _solutionService = new SolutionService(_mockRepositoryWrapper.Object);
        }

        [Fact]
        public async Task GetByTaskId_WithValidTaskId_ShouldReturnSolutions()
        {
            // Arrange
            var taskId = 1;
            var expectedSolutions = new List<Solution>
            {
                new Solution
                {
                    Id = 1,
                    TaskId = taskId,
                    SolutionText = "Solution 1",
                    DateAdded = DateTime.Now,
                    UserAddedId = 1,
                    UserAdded = new User { Id = 1, Username = "user1", Role = new Role { Id = 1, RoleName = "User" } }
                },
                new Solution
                {
                    Id = 2,
                    TaskId = taskId,
                    SolutionText = "Solution 2",
                    DateAdded = DateTime.Now,
                    UserAddedId = 2,
                    UserAdded = new User { Id = 2, Username = "user2", Role = new Role { Id = 1, RoleName = "User" } }
                }
            };

            _mockSolutionRepository
                .Setup(x => x.GetSolutionsByTaskId(taskId))
                .ReturnsAsync(expectedSolutions);

            // Act
            var result = await _solutionService.GetByTaskId(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedSolutions, result);
            _mockSolutionRepository.Verify(x => x.GetSolutionsByTaskId(taskId), Times.Once);
        }

        [Fact]
        public async Task GetByTaskId_WithInvalidTaskId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidTaskId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _solutionService.GetByTaskId(invalidTaskId));

            _mockSolutionRepository.Verify(x => x.GetSolutionsByTaskId(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetByTaskId_WithNegativeTaskId_ShouldThrowArgumentException()
        {
            // Arrange
            var negativeTaskId = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _solutionService.GetByTaskId(negativeTaskId));

            _mockSolutionRepository.Verify(x => x.GetSolutionsByTaskId(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetByTaskId_WhenNoSolutions_ShouldReturnEmptyList()
        {
            // Arrange
            var taskId = 1;
            var emptyList = new List<Solution>();

            _mockSolutionRepository
                .Setup(x => x.GetSolutionsByTaskId(taskId))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _solutionService.GetByTaskId(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockSolutionRepository.Verify(x => x.GetSolutionsByTaskId(taskId), Times.Once);
        }

        [Fact]
        public async Task GetByTaskId_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var taskId = 1;

            _mockSolutionRepository
                .Setup(x => x.GetSolutionsByTaskId(taskId))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _solutionService.GetByTaskId(taskId));
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnSolution()
        {
            // Arrange
            var solutionId = 1;
            var expectedSolution = new Solution
            {
                Id = solutionId,
                TaskId = 1,
                SolutionText = "Test Solution",
                DateAdded = DateTime.Now,
                UserAddedId = 1,
                UserAdded = new User { Id = 1, Username = "user1", Role = new Role { Id = 1, RoleName = "User" } }
            };

            _mockSolutionRepository
                .Setup(x => x.GetById(solutionId))
                .ReturnsAsync(expectedSolution);

            // Act
            var result = await _solutionService.GetById(solutionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSolution, result);
            Assert.NotNull(result.UserAdded);
            Assert.NotNull(result.UserAdded.Role);
            _mockSolutionRepository.Verify(x => x.GetById(solutionId), Times.Once);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _solutionService.GetById(invalidId));

            _mockSolutionRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetById_WithNegativeId_ShouldThrowArgumentException()
        {
            // Arrange
            var negativeId = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _solutionService.GetById(negativeId));

            _mockSolutionRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetById_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var solutionId = 1;

            _mockSolutionRepository
                .Setup(x => x.GetById(solutionId))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _solutionService.GetById(solutionId));
        }

        [Fact]
        public async Task Create_ShouldCallRepositoryCreateAndSave()
        {
            // Arrange
            var solution = new Solution
            {
                TaskId = 1,
                UserAddedId = 1,
                SolutionText = "New Solution",
                DateAdded = DateTime.Now
            };

            _mockSolutionRepository
                .Setup(x => x.Create(solution))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _solutionService.Create(solution);

            // Assert
            _mockSolutionRepository.Verify(x => x.Create(solution), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _solutionService.Create(null));

            _mockSolutionRepository.Verify(x => x.Create(It.IsAny<Solution>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_WhenSaveFails_ShouldPropagateException()
        {
            // Arrange
            var solution = new Solution
            {
                TaskId = 1,
                UserAddedId = 1,
                SolutionText = "New Solution",
                DateAdded = DateTime.Now
            };

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .ThrowsAsync(new Exception("Save failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _solutionService.Create(solution));

            _mockSolutionRepository.Verify(x => x.Create(solution), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdateAndSave()
        {
            // Arrange
            var solution = new Solution
            {
                Id = 1,
                TaskId = 1,
                UserAddedId = 1,
                SolutionText = "Updated Solution",
                DateAdded = DateTime.Now
            };

            _mockSolutionRepository
                .Setup(x => x.Update(solution))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _solutionService.Update(solution);

            // Assert
            _mockSolutionRepository.Verify(x => x.Update(solution), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _solutionService.Update(null));

            _mockSolutionRepository.Verify(x => x.Update(It.IsAny<Solution>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldCallRepositoryDeleteAndSave()
        {
            // Arrange
            var solutionId = 1;
            var solution = new Solution
            {
                Id = solutionId,
                TaskId = 1,
                UserAddedId = 1,
                SolutionText = "Solution",
                DateAdded = DateTime.Now
            };
            var solutions = new List<Solution> { solution };

            _mockSolutionRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Solution, bool>>>()))
                .ReturnsAsync(solutions);

            _mockSolutionRepository
                .Setup(x => x.Delete(solution))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _solutionService.Delete(solutionId);

            // Assert
            _mockSolutionRepository.Verify(
                x => x.FindByCondition(It.IsAny<Expression<Func<Solution, bool>>>()),
                Times.Once);
            _mockSolutionRepository.Verify(x => x.Delete(solution), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var nonExistingId = 999;
            var emptyList = new List<Solution>();

            _mockSolutionRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Solution, bool>>>()))
                .ReturnsAsync(emptyList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _solutionService.Delete(nonExistingId));

            _mockSolutionRepository.Verify(x => x.Delete(It.IsAny<Solution>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _solutionService.Delete(invalidId));

            _mockSolutionRepository.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Solution, bool>>>()), Times.Never);
            _mockSolutionRepository.Verify(x => x.Delete(It.IsAny<Solution>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenMultipleRecordsFound_ShouldDeleteFirstOne()
        {
            // Arrange
            var solutionId = 1;
            var solutions = new List<Solution>
            {
                new Solution { Id = solutionId, TaskId = 1, SolutionText = "First", DateAdded = DateTime.Now },
                new Solution { Id = solutionId, TaskId = 1, SolutionText = "Second", DateAdded = DateTime.Now }
            };

            _mockSolutionRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Solution, bool>>>()))
                .ReturnsAsync(solutions);

            _mockSolutionRepository
                .Setup(x => x.Delete(It.IsAny<Solution>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _solutionService.Delete(solutionId);

            // Assert
            _mockSolutionRepository.Verify(x => x.Delete(solutions.First()), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenFindByConditionReturnsNull_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var solutionId = 1;
            List<Solution> nullList = null;

            _mockSolutionRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Solution, bool>>>()))
                .ReturnsAsync(nullList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _solutionService.Delete(solutionId));

            _mockSolutionRepository.Verify(x => x.Delete(It.IsAny<Solution>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public void Constructor_WithNullRepositoryWrapper_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SolutionService(null));
        }

        [Fact]
        public void Constructor_WithValidRepositoryWrapper_ShouldInitializeService()
        {
            // Arrange
            var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            // Act
            var service = new SolutionService(mockRepositoryWrapper.Object);

            // Assert
            Assert.NotNull(service);
        }
    }
}