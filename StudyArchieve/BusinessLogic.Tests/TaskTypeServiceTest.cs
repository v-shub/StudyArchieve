using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogic.Tests
{
    public class TaskTypeServiceTest
    {
        private readonly TaskTypeService _service;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;
        private readonly Mock<ITaskTypeRepository> _taskTypeRepositoryMoq;

        public TaskTypeServiceTest()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _taskTypeRepositoryMoq = new Mock<ITaskTypeRepository>();

            _repositoryWrapperMoq.Setup(x => x.TaskType)
                                .Returns(_taskTypeRepositoryMoq.Object);

            _service = new TaskTypeService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllTaskTypes()
        {
            // Arrange
            var taskTypes = new List<TaskType>
        {
            new TaskType { Id = 1, Name = "Лабораторная работа" },
            new TaskType { Id = 2, Name = "Домашнее задание" },
            new TaskType { Id = 3, Name = "Экзаменационный билет" }
        };

            _taskTypeRepositoryMoq.Setup(x => x.FindAll())
                                 .ReturnsAsync(taskTypes);

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal("Лабораторная работа", result[0].Name);

            Assert.Equal(2, result[1].Id);
            Assert.Equal("Домашнее задание", result[1].Name);

            Assert.Equal(3, result[2].Id);
            Assert.Equal("Экзаменационный билет", result[2].Name);

            _taskTypeRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _taskTypeRepositoryMoq.Setup(x => x.FindAll())
                                 .ReturnsAsync(new List<TaskType>());

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _taskTypeRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_RepositoryThrowsException_ThrowsSameException()
        {
            // Arrange
            var expectedException = new Exception("Database error");
            _taskTypeRepositoryMoq.Setup(x => x.FindAll())
                                 .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAll());
            Assert.Equal("Database error", exception.Message);

            _taskTypeRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }
    }
}
