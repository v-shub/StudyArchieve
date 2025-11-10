using Domain.Interfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Exercise = Domain.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Services.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockAuthorRepository = new Mock<IAuthorRepository>();
            _mockTagRepository = new Mock<ITagRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.Exercise)
                .Returns(_mockTaskRepository.Object);
            _mockRepositoryWrapper
                .Setup(x => x.Author)
                .Returns(_mockAuthorRepository.Object);
            _mockRepositoryWrapper
                .Setup(x => x.Tag)
                .Returns(_mockTagRepository.Object);

            _taskService = new TaskService(_mockRepositoryWrapper.Object);
        }

        [Fact]
        public async Task GetByFilter_WithNoFilters_ShouldReturnAllTasks()
        {
            // Arrange
            var expectedTasks = new List<Exercise>
            {
                new Exercise { Id = 1, Title = "Task 1", SubjectId = 1, AcademicYearId = 1, TypeId = 1 },
                new Exercise { Id = 2, Title = "Task 2", SubjectId = 2, AcademicYearId = 2, TypeId = 2 }
            };

            _mockTaskRepository
                .Setup(x => x.GetTasksWithDetails())
                .ReturnsAsync(expectedTasks);

            // Act
            var result = await _taskService.GetByFilter();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockTaskRepository.Verify(x => x.GetTasksWithDetails(), Times.Once);
        }

        [Fact]
        public async Task GetByFilter_WithSubjectFilter_ShouldReturnFilteredTasks()
        {
            // Arrange
            var subjectId = 1;
            var tasks = new List<Exercise>
            {
                new Exercise { Id = 1, Title = "Task 1", SubjectId = 1 },
                new Exercise { Id = 2, Title = "Task 2", SubjectId = 2 }
            };

            _mockTaskRepository
                .Setup(x => x.GetTasksWithDetails())
                .ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetByFilter(subjectId: subjectId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(subjectId, result[0].SubjectId);
        }

        [Fact]
        public async Task GetByFilter_WithAcademicYearFilter_ShouldReturnFilteredTasks()
        {
            // Arrange
            var academicYearId = 1;
            var tasks = new List<Exercise>
            {
                new Exercise { Id = 1, Title = "Task 1", AcademicYearId = 1 },
                new Exercise { Id = 2, Title = "Task 2", AcademicYearId = 2 }
            };

            _mockTaskRepository
                .Setup(x => x.GetTasksWithDetails())
                .ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetByFilter(academicYearId: academicYearId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(academicYearId, result[0].AcademicYearId);
        }

        [Fact]
        public async Task GetByFilter_WithTypeFilter_ShouldReturnFilteredTasks()
        {
            // Arrange
            var typeId = 1;
            var tasks = new List<Exercise>
            {
                new Exercise { Id = 1, Title = "Task 1", TypeId = 1 },
                new Exercise { Id = 2, Title = "Task 2", TypeId = 2 }
            };

            _mockTaskRepository
                .Setup(x => x.GetTasksWithDetails())
                .ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetByFilter(typeId: typeId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(typeId, result[0].TypeId);
        }

        [Fact]
        public async Task GetByFilter_WithAuthorFilter_ShouldReturnTasksWithAllAuthors()
        {
            // Arrange
            var authorIds = new[] { 1, 2 };
            var tasks = new List<Exercise>
            {
                new Exercise
                {
                    Id = 1,
                    Title = "Task 1",
                    Authors = new List<Author>
                    {
                        new Author { Id = 1 },
                        new Author { Id = 2 }
                    }
                },
                new Exercise
                {
                    Id = 2,
                    Title = "Task 2",
                    Authors = new List<Author>
                    {
                        new Author { Id = 1 }
                    }
                }
            };

            _mockTaskRepository
                .Setup(x => x.GetTasksWithDetails())
                .ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetByFilter(authorIds: authorIds);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public async Task GetByFilter_WithTagFilter_ShouldReturnTasksWithAllTags()
        {
            // Arrange
            var tagIds = new[] { 1, 2 };
            var tasks = new List<Exercise>
            {
                new Exercise
                {
                    Id = 1,
                    Title = "Task 1",
                    Tags = new List<Tag>
                    {
                        new Tag { Id = 1 },
                        new Tag { Id = 2 }
                    }
                },
                new Exercise
                {
                    Id = 2,
                    Title = "Task 2",
                    Tags = new List<Tag>
                    {
                        new Tag { Id = 1 }
                    }
                }
            };

            _mockTaskRepository
                .Setup(x => x.GetTasksWithDetails())
                .ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetByFilter(tagIds: tagIds);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public async Task GetByFilter_WithMultipleFilters_ShouldReturnCorrectlyFilteredTasks()
        {
            // Arrange
            var subjectId = 1;
            var academicYearId = 1;
            var tasks = new List<Exercise>
            {
                new Exercise { Id = 1, Title = "Task 1", SubjectId = 1, AcademicYearId = 1 },
                new Exercise { Id = 2, Title = "Task 2", SubjectId = 1, AcademicYearId = 2 },
                new Exercise { Id = 3, Title = "Task 3", SubjectId = 2, AcademicYearId = 1 }
            };

            _mockTaskRepository
                .Setup(x => x.GetTasksWithDetails())
                .ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetByFilter(subjectId: subjectId, academicYearId: academicYearId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnTask()
        {
            // Arrange
            var taskId = 1;
            var expectedTask = new Exercise
            {
                Id = taskId,
                Title = "Test Task",
                Subject = new Subject { Id = 1, Name = "Math" },
                Type = new TaskType { Id = 1, Name = "Homework" },
                UserAdded = new User { Id = 1, Username = "user1", Role = new Role { Id = 1, RoleName = "User" } }
            };

            _mockTaskRepository
                .Setup(x => x.GetOneTaskWithDetails(taskId))
                .ReturnsAsync(expectedTask);

            // Act
            var result = await _taskService.GetById(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTask, result);
            _mockTaskRepository.Verify(x => x.GetOneTaskWithDetails(taskId), Times.Once);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.GetById(invalidId));

            _mockTaskRepository.Verify(x => x.GetOneTaskWithDetails(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetById_WithNegativeId_ShouldThrowArgumentException()
        {
            // Arrange
            var negativeId = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.GetById(negativeId));

            _mockTaskRepository.Verify(x => x.GetOneTaskWithDetails(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetById_WhenTaskNotFound_ShouldReturnNull()
        {
            // Arrange
            var taskId = 999;

            _mockTaskRepository
                .Setup(x => x.GetOneTaskWithDetails(taskId))
                .ReturnsAsync((Exercise)null);

            // Act
            var result = await _taskService.GetById(taskId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Create_WithAuthorsAndTags_ShouldCreateTaskWithRelations()
        {
            // Arrange
            var task = new Exercise
            {
                Title = "New Task",
                Authors = new List<Author> { new Author { Id = 1 }, new Author { Id = 2 } },
                Tags = new List<Tag> { new Tag { Id = 1 }, new Tag { Id = 2 } }
            };

            var existingAuthors = new List<Author> { new Author { Id = 1 }, new Author { Id = 2 } };
            var existingTags = new List<Tag> { new Tag { Id = 1 }, new Tag { Id = 2 } };

            _mockAuthorRepository
                .Setup(x => x.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(existingAuthors);

            _mockTagRepository
                .Setup(x => x.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(existingTags);

            _mockAuthorRepository
                .Setup(x => x.AttachAsync(It.IsAny<Author>()))
                .Returns(Task.CompletedTask);

            _mockTagRepository
                .Setup(x => x.AttachAsync(It.IsAny<Tag>()))
                .Returns(Task.CompletedTask);

            _mockTaskRepository
                .Setup(x => x.Create(It.IsAny<Exercise>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _taskService.Create(task);

            // Assert
            _mockAuthorRepository.Verify(x => x.GetByIdsAsync(It.IsAny<List<int>>()), Times.Once);
            _mockTagRepository.Verify(x => x.GetByIdsAsync(It.IsAny<List<int>>()), Times.Once);
            _mockTaskRepository.Verify(x => x.Create(It.IsAny<Exercise>()), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _taskService.Create(null));

            _mockTaskRepository.Verify(x => x.Create(It.IsAny<Exercise>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_WithoutAuthorsAndTags_ShouldCreateTask()
        {
            // Arrange
            var task = new Exercise { Title = "New Task" };

            _mockTaskRepository
                .Setup(x => x.Create(It.IsAny<Exercise>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _taskService.Create(task);

            // Assert
            _mockAuthorRepository.Verify(x => x.GetByIdsAsync(It.IsAny<List<int>>()), Times.Never);
            _mockTagRepository.Verify(x => x.GetByIdsAsync(It.IsAny<List<int>>()), Times.Never);
            _mockTaskRepository.Verify(x => x.Create(It.IsAny<Exercise>()), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_WithValidTask_ShouldUpdateTaskAndRelations()
        {
            // Arrange
            var taskId = 1;
            var existingTask = new Exercise
            {
                Id = taskId,
                Title = "Old Title",
                Authors = new List<Author>(),
                Tags = new List<Tag>()
            };

            var updatedTask = new Exercise
            {
                Id = taskId,
                Title = "Updated Title",
                Authors = new List<Author> { new Author { Id = 1 } },
                Tags = new List<Tag> { new Tag { Id = 1 } }
            };

            var existingAuthors = new List<Author> { new Author { Id = 1 } };
            var existingTags = new List<Tag> { new Tag { Id = 1 } };

            _mockTaskRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Exercise, bool>>>()))
                .ReturnsAsync(new List<Exercise> { existingTask });

            _mockAuthorRepository
                .Setup(x => x.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(existingAuthors);

            _mockTagRepository
                .Setup(x => x.GetByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(existingTags);

            _mockAuthorRepository
                .Setup(x => x.AttachAsync(It.IsAny<Author>()))
                .Returns(Task.CompletedTask);

            _mockTagRepository
                .Setup(x => x.AttachAsync(It.IsAny<Tag>()))
                .Returns(Task.CompletedTask);

            _mockTaskRepository
                .Setup(x => x.Update(It.IsAny<Exercise>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _taskService.Update(updatedTask);

            // Assert
            Assert.Equal("Updated Title", existingTask.Title);
            _mockTaskRepository.Verify(x => x.Update(existingTask), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_WithNonExistingTask_ShouldThrowArgumentException()
        {
            // Arrange
            var task = new Exercise { Id = 999 };

            _mockTaskRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Exercise, bool>>>()))
                .ReturnsAsync(new List<Exercise>());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.Update(task));

            _mockTaskRepository.Verify(x => x.Update(It.IsAny<Exercise>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Update_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _taskService.Update(null));

            _mockTaskRepository.Verify(x => x.Update(It.IsAny<Exercise>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldCallRepositoryDeleteAndSave()
        {
            // Arrange
            var taskId = 1;
            var task = new Exercise { Id = taskId, Title = "Task" };
            var tasks = new List<Exercise> { task };

            _mockTaskRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Exercise, bool>>>()))
                .ReturnsAsync(tasks);

            _mockTaskRepository
                .Setup(x => x.Delete(task))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _taskService.Delete(taskId);

            // Assert
            _mockTaskRepository.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Exercise, bool>>>()), Times.Once);
            _mockTaskRepository.Verify(x => x.Delete(task), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var nonExistingId = 999;
            var emptyList = new List<Exercise>();

            _mockTaskRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Exercise, bool>>>()))
                .ReturnsAsync(emptyList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.Delete(nonExistingId));

            _mockTaskRepository.Verify(x => x.Delete(It.IsAny<Exercise>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.Delete(invalidId));

            _mockTaskRepository.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Exercise, bool>>>()), Times.Never);
            _mockTaskRepository.Verify(x => x.Delete(It.IsAny<Exercise>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public void Constructor_WithNullRepositoryWrapper_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TaskService(null));
        }

        [Fact]
        public void Constructor_WithValidRepositoryWrapper_ShouldInitializeService()
        {
            // Arrange
            var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            // Act
            var service = new TaskService(mockRepositoryWrapper.Object);

            // Assert
            Assert.NotNull(service);
        }
    }
}