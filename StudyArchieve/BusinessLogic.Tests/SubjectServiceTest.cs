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
    public class SubjectServiceTest
    {
        private readonly SubjectService _service;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;
        private readonly Mock<ISubjectRepository> _subjectRepositoryMoq;

        public SubjectServiceTest()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _subjectRepositoryMoq = new Mock<ISubjectRepository>();

            _repositoryWrapperMoq.Setup(x => x.Subject)
                                .Returns(_subjectRepositoryMoq.Object);

            _service = new SubjectService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllSubjects()
        {
            // Arrange
            var subjects = new List<Subject>
        {
            new Subject { Id = 1, Name = "Mathematics" },
            new Subject { Id = 2, Name = "Physics" },
            new Subject { Id = 3, Name = "Programming" }
        };

            _subjectRepositoryMoq.Setup(x => x.FindAll())
                                .ReturnsAsync(subjects);

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal("Mathematics", result[0].Name);

            Assert.Equal(2, result[1].Id);
            Assert.Equal("Physics", result[1].Name);

            Assert.Equal(3, result[2].Id);
            Assert.Equal("Programming", result[2].Name);

            _subjectRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _subjectRepositoryMoq.Setup(x => x.FindAll())
                                .ReturnsAsync(new List<Subject>());

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _subjectRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_RepositoryThrowsException_ThrowsSameException()
        {
            // Arrange
            var expectedException = new Exception("Database error");
            _subjectRepositoryMoq.Setup(x => x.FindAll())
                                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAll());
            Assert.Equal("Database error", exception.Message);

            _subjectRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }
    }
}
