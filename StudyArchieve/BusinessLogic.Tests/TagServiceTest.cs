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
    public class TagServiceTest
    {
        private readonly TagService _service;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;
        private readonly Mock<ITagRepository> _tagRepositoryMoq;

        public TagServiceTest()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _tagRepositoryMoq = new Mock<ITagRepository>();

            _repositoryWrapperMoq.Setup(x => x.Tag)
                                .Returns(_tagRepositoryMoq.Object);

            _service = new TagService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllTags()
        {
            // Arrange
            var tags = new List<Tag>
        {
            new Tag { Id = 1, Name = "C#" },
            new Tag { Id = 2, Name = "SQL" },
            new Tag { Id = 3, Name = "OOP" }
        };

            _tagRepositoryMoq.Setup(x => x.FindAll())
                            .ReturnsAsync(tags);

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal("C#", result[0].Name);

            Assert.Equal(2, result[1].Id);
            Assert.Equal("SQL", result[1].Name);

            Assert.Equal(3, result[2].Id);
            Assert.Equal("OOP", result[2].Name);

            _tagRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _tagRepositoryMoq.Setup(x => x.FindAll())
                            .ReturnsAsync(new List<Tag>());

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _tagRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_RepositoryThrowsException_ThrowsSameException()
        {
            // Arrange
            var expectedException = new Exception("Database error");
            _tagRepositoryMoq.Setup(x => x.FindAll())
                            .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAll());
            Assert.Equal("Database error", exception.Message);

            _tagRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }
    }
}
