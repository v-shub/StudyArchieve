using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise = Domain.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Tests
{
    public class TaskServiceTest
    {
        private readonly TaskService service;
        private readonly Mock<ITaskRepository> taskRepositoryMoq;
        public TaskServiceTest()
        {
            var repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            taskRepositoryMoq = new Mock<ITaskRepository>();
            repositoryWrapperMoq.Setup(x => x.Exercise)
                .Returns(taskRepositoryMoq.Object);
            service = new TaskService(repositoryWrapperMoq.Object);
        }
        /*
        [Fact]
        public async Task GetById_ImpossibleId_ShouldThrowArgumentException()
        {
            var ex1 = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.GetById(0));
            var ex2 = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.GetById(-3));

            Assert.IsType<ArgumentException>(ex1);
            Assert.IsType<ArgumentException>(ex2);
            taskRepositoryMoq.Verify(x => x.GetOneTaskWithAllConnected(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetById_WrongId_ShouldReturnNull()
        {
            var res = await service.GetById(100000000);

            Assert.Null(res);
            taskRepositoryMoq.Verify(x => x.GetOneTaskWithAllConnected(It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public async Task GetById_CorrectId_ShouldReturnFullTaskDto()
        {
            var taskId = 1;
            var subject = new Subject();
            subject.Id = 2;
            subject.Name = "subject";
            var acYear = new AcademicYear();
            acYear.Id = 3;
            acYear.YearLabel = "2020-2021";
            var taskType = new TaskType();
            taskType.Id = 4;
            taskType.Name = "type";
            var exercise = new Exercise
            {
                Id = taskId,
                Title = "Test Task",
                ConditionText = "Test condition",
                SubjectId = subject.Id,
                Subject = subject,
                AcademicYearId = acYear.Id,
                AcademicYear = acYear,
                TypeId = taskType.Id,
                Type = taskType,
                DateAdded = DateTime.Today,
                Authors = new List<Author>(),
                Tags = new List<Tag>(),
                Solutions = new List<Solution>(),
                TaskFiles = new List<TaskFile>()
            };

            var expectedDto = new FullTaskDto
            {
                Id = taskId,
                Title = exercise.Title,
                ConditionText = exercise.ConditionText,
                SubjectId = exercise.SubjectId,
                SubjectName = exercise.Subject.Name,
                AcademicYearId = exercise.AcademicYearId,
                AcademicYearLabel = exercise.AcademicYear.YearLabel,
                TypeId = exercise.TypeId,
                TypeName = exercise.Type.Name,
                DateAdded = DateTime.Today,
                Authors = new List<AuthorDto>(),
                Tags = new List<TagDto>(),
                Solutions = new List<SolutionDto>(),
                TaskFiles = new List<TaskFileDto>()
            };
            taskRepositoryMoq.Setup(x => x.GetOneTaskWithAllConnected(taskId))
                    .ReturnsAsync(exercise);

            var result = await service.GetById(taskId);

            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal("Test Task", result.Title);
            Assert.Equal("Test condition", result.ConditionText);
            Assert.Equal(subject.Id, result.SubjectId);
            Assert.Equal(subject.Name, result.SubjectName);
            Assert.Equal(acYear.Id, result.AcademicYearId);
            Assert.Equal(acYear.YearLabel, result.AcademicYearLabel);
            Assert.Equal(taskType.Id, result.TypeId);
            Assert.Equal(taskType.Name, result.TypeName);
            Assert.Equal(DateTime.Today, result.DateAdded);
            Assert.NotNull(result.Authors);
            Assert.NotNull(result.Tags);
            Assert.NotNull(result.Solutions);
            Assert.NotNull(result.TaskFiles);
            taskRepositoryMoq.Verify(x => x.GetOneTaskWithAllConnected(It.IsAny<int>()), Times.Once);
        }
        */
        [Fact]
        public async Task GetById_ValidId_ReturnsFullTaskDto()
        {
            // Arrange
            var taskId = 1;
            var exercise = new Exercise
            {
                Id = taskId,
                Title = "Test Task",
                Subject = new Subject { Id = 1, Name = "Math" },
                AcademicYear = new AcademicYear { Id = 1, YearLabel = "2023-2024" },
                Type = new TaskType { Id = 1, Name = "Lab" },
                Authors = new List<Author>(),
                Tags = new List<Tag>(),
                Solutions = new List<Solution>(),
                TaskFiles = new List<TaskFile>()
            };

            taskRepositoryMoq.Setup(x => x.GetOneTaskWithAllConnected(taskId))
                             .ReturnsAsync(exercise);

            // Act
            var result = await service.GetById(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(taskId, result.Id);
            Assert.Equal("Test Task", result.Title);
            taskRepositoryMoq.Verify(x => x.GetOneTaskWithAllConnected(taskId), Times.Once);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNull()
        {
            // Arrange
            var taskId = 999;

            taskRepositoryMoq.Setup(x => x.GetOneTaskWithAllConnected(taskId))
                             .ReturnsAsync((Exercise)null);

            // Act
            var result = await service.GetById(taskId);

            // Assert
            Assert.Null(result);
            taskRepositoryMoq.Verify(x => x.GetOneTaskWithAllConnected(taskId), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetById_InvalidId_ThrowsArgumentException(int invalidId)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetById(invalidId));
            taskRepositoryMoq.Verify(x => x.GetOneTaskWithAllConnected(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetById_RepositoryThrowsException_ThrowsSameException()
        {
            // Arrange
            var taskId = 1;
            var expectedException = new Exception("Database error");

            taskRepositoryMoq.Setup(x => x.GetOneTaskWithAllConnected(taskId))
                             .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => service.GetById(taskId));
            Assert.Equal("Database error", exception.Message);
            taskRepositoryMoq.Verify(x => x.GetOneTaskWithAllConnected(taskId), Times.Once);
        }

        [Fact]
        public async Task GetById_WithCollections_ReturnsDtoWithMappedCollections()
        {
            // Arrange
            var taskId = 1;
            var exercise = new Exercise
            {
                Id = taskId,
                Title = "Test Task",
                Authors = new List<Author> { new Author { Id = 1, Name = "Author 1" } },
                Tags = new List<Tag> { new Tag { Id = 1, Name = "Tag 1" } },
                Solutions = new List<Solution> { new Solution { Id = 1, SolutionText = "Solution 1" } },
                TaskFiles = new List<TaskFile> { new TaskFile { Id = 1, FileName = "file.txt" } }
            };

            taskRepositoryMoq.Setup(x => x.GetOneTaskWithAllConnected(taskId))
                             .ReturnsAsync(exercise);

            // Act
            var result = await service.GetById(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Authors);
            Assert.Single(result.Tags);
            Assert.Single(result.Solutions);
            Assert.Single(result.TaskFiles);
            taskRepositoryMoq.Verify(x => x.GetOneTaskWithAllConnected(taskId), Times.Once);
        }

        public static IEnumerable<object[]> FilterTestData()
        {
            // Все задачи
            yield return new object[] { null, null, null, null, null, 3 };

            // Фильтр по subjectId
            yield return new object[] { 1, null, null, null, null, 2 };
            yield return new object[] { 2, null, null, null, null, 1 };

            // Фильтр по academicYearId
            yield return new object[] { null, 3, null, null, null, 2 };
            yield return new object[] { null, 4, null, null, null, 1 };

            // Фильтр по typeId
            yield return new object[] { null, null, 1, null, null, 2 };
            yield return new object[] { null, null, 2, null, null, 1 };

            // Комбинированные фильтры
            yield return new object[] { 1, 3, 1, null, null, 1 };
        }

        public static IEnumerable<object[]> AuthorFilterTestData()
        {
            // Фильтр по authorIds
            yield return new object[] { new int[] { 1 }, 2 }; // Задачи где есть автор 1
            yield return new object[] { new int[] { 2 }, 1 }; // Задачи где есть автор 2
            yield return new object[] { new int[] { 1, 2 }, 1 }; // Задачи где есть оба автора
            yield return new object[] { new int[] { 300 }, 0 }; // Несуществующий автор
        }

        public static IEnumerable<object[]> TagFilterTestData()
        {
            // Фильтр по tagIds
            yield return new object[] { new int[] { 1 }, 2 }; // Задачи где есть тег 1
            yield return new object[] { new int[] { 2 }, 1 }; // Задачи где есть тег 2
            yield return new object[] { new int[] { 1, 2 }, 1 }; // Задачи где есть оба тега
            yield return new object[] { new int[] { 300 }, 0 }; // Несуществующий тег
        }

        private List<Exercise> GetTestTasks()
        {
            return new List<Exercise>
        {
            new Exercise
            {
                Id = 1,
                Title = "Task 1",
                SubjectId = 1,
                AcademicYearId = 3,
                TypeId = 1,
                Authors = new List<Author> { new Author { Id = 1 }, new Author { Id = 2 } },
                Tags = new List<Tag> { new Tag { Id = 1 }, new Tag { Id = 2 } }
            },
            new Exercise
            {
                Id = 2,
                Title = "Task 2",
                SubjectId = 1,
                AcademicYearId = 3,
                TypeId = 2,
                Authors = new List<Author> { new Author { Id = 1 } },
                Tags = new List<Tag> { new Tag { Id = 1 } }
            },
            new Exercise
            {
                Id = 3,
                Title = "Task 3",
                SubjectId = 2,
                AcademicYearId = 4,
                TypeId = 1,
                Authors = new List<Author> { new Author { Id = 3 } },
                Tags = new List<Tag> { new Tag { Id = 3 } }
            }
        };
        }

        [Theory]
        [MemberData(nameof(FilterTestData))]
        public async Task GetByFilter_VariousFilters_ReturnsCorrectCount(
            int? subjectId, int? academicYearId, int? typeId, int[] authorIds, int[] tagIds, int expectedCount)
        {
            // Arrange
            var tasks = GetTestTasks();
            taskRepositoryMoq.Setup(x => x.GetTasksWithDetails()).ReturnsAsync(tasks);

            // Act
            var result = await service.GetByFilter(subjectId, academicYearId, typeId, authorIds, tagIds);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count);
            taskRepositoryMoq.Verify(x => x.GetTasksWithDetails(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(AuthorFilterTestData))]
        public async Task GetByFilter_AuthorIdsFilter_ReturnsCorrectTasks(int[] authorIds, int expectedCount)
        {
            // Arrange
            var tasks = GetTestTasks();
            taskRepositoryMoq.Setup(x => x.GetTasksWithDetails()).ReturnsAsync(tasks);

            // Act
            var result = await service.GetByFilter(authorIds: authorIds);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count);

            if (expectedCount > 0)
            {
                Assert.All(result, taskDto =>
                {
                    var task = tasks.First(t => t.Id == taskDto.Id);
                    Assert.True(authorIds.All(id => task.Authors.Any(a => a.Id == id)));
                });
            }
        }

        [Theory]
        [MemberData(nameof(TagFilterTestData))]
        public async Task GetByFilter_TagIdsFilter_ReturnsCorrectTasks(int[] tagIds, int expectedCount)
        {
            // Arrange
            var tasks = GetTestTasks();
            taskRepositoryMoq.Setup(x => x.GetTasksWithDetails()).ReturnsAsync(tasks);

            // Act
            var result = await service.GetByFilter(tagIds: tagIds);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count);

            if (expectedCount > 0)
            {
                Assert.All(result, taskDto =>
                {
                    var task = tasks.First(t => t.Id == taskDto.Id);
                    Assert.True(tagIds.All(id => task.Tags.Any(t => t.Id == id)));
                });
            }
        }

        [Fact]
        public async Task GetByFilter_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            taskRepositoryMoq.Setup(x => x.GetTasksWithDetails()).ReturnsAsync(new List<Exercise>());

            // Act
            var result = await service.GetByFilter();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            taskRepositoryMoq.Verify(x => x.GetTasksWithDetails(), Times.Once);
        }

        [Fact]
        public async Task GetByFilter_NullArrays_DoesNotThrow()
        {
            // Arrange
            var tasks = GetTestTasks();
            taskRepositoryMoq.Setup(x => x.GetTasksWithDetails()).ReturnsAsync(tasks);

            // Act
            var result = await service.GetByFilter(authorIds: null, tagIds: null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            taskRepositoryMoq.Verify(x => x.GetTasksWithDetails(), Times.Once);
        }

        [Fact]
        public async Task GetByFilter_EmptyArrays_ReturnsAllTasks()
        {
            // Arrange
            var tasks = GetTestTasks();
            taskRepositoryMoq.Setup(x => x.GetTasksWithDetails()).ReturnsAsync(tasks);

            // Act
            var result = await service.GetByFilter(authorIds: new int[0], tagIds: new int[0]);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            taskRepositoryMoq.Verify(x => x.GetTasksWithDetails(), Times.Once);
        }
    }
}
