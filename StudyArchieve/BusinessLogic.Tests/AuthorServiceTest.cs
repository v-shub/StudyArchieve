/*using BusinessLogic.Services;
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
    public class AuthorServiceTest
    {
        private readonly AuthorService _service;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;
        private readonly Mock<IAuthorRepository> _authorRepositoryMoq;

        public AuthorServiceTest()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _authorRepositoryMoq = new Mock<IAuthorRepository>();

            _repositoryWrapperMoq.Setup(x => x.Author)
                                .Returns(_authorRepositoryMoq.Object);

            _service = new AuthorService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllAuthors()
        {
            // Arrange
            var authors = new List<Author>
        {
            new Author { Id = 1, Name = "Author 1" },
            new Author { Id = 2, Name = "Author 2" },
            new Author { Id = 3, Name = "Author 3" }
        };

            _authorRepositoryMoq.Setup(x => x.FindAll())
                               .ReturnsAsync(authors);

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal("Author 1", result[0].Name);

            Assert.Equal(2, result[1].Id);
            Assert.Equal("Author 2", result[1].Name);

            Assert.Equal(3, result[2].Id);
            Assert.Equal("Author 3", result[2].Name);

            _authorRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _authorRepositoryMoq.Setup(x => x.FindAll())
                               .ReturnsAsync(new List<Author>());

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _authorRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_RepositoryThrowsException_ThrowsSameException()
        {
            // Arrange
            var expectedException = new Exception("Database error");
            _authorRepositoryMoq.Setup(x => x.FindAll())
                               .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAll());
            Assert.Equal("Database error", exception.Message);

            _authorRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }
    }
}
*/