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
    public class AuthorServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IAuthorRepository> _mockAuthorRepository;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockAuthorRepository = new Mock<IAuthorRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.Author)
                .Returns(_mockAuthorRepository.Object);

            _authorService = new AuthorService(_mockRepositoryWrapper.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllAuthors()
        {
            // Arrange
            var expectedAuthors = new List<Author>
            {
                new Author { Id = 1, Name = "Author 1" },
                new Author { Id = 2, Name = "Author 2" }
            };

            _mockAuthorRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(expectedAuthors);

            // Act
            var result = await _authorService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedAuthors, result);
            _mockAuthorRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyList = new List<Author>();

            _mockAuthorRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _authorService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockAuthorRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            _mockAuthorRepository
                .Setup(x => x.FindAll())
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _authorService.GetAll());
        }

        [Fact]
        public async Task Create_ShouldCallRepositoryCreateAndSave()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Author 1" };

            _mockAuthorRepository
                .Setup(x => x.Create(author))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _authorService.Create(author);

            // Assert
            _mockAuthorRepository.Verify(x => x.Create(author), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _authorService.Create(null));

            _mockAuthorRepository.Verify(x => x.Create(It.IsAny<Author>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_WhenSaveFails_ShouldNotCallCreate()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Author 1" };

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .ThrowsAsync(new Exception("Save failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _authorService.Create(author));

            _mockAuthorRepository.Verify(x => x.Create(author), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdateAndSave()
        {
            // Arrange
            var author = new Author { Id = 1, Name = "Updated Author" };

            _mockAuthorRepository
                .Setup(x => x.Update(author))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _authorService.Update(author);

            // Assert
            _mockAuthorRepository.Verify(x => x.Update(author), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _authorService.Update(null));

            _mockAuthorRepository.Verify(x => x.Update(It.IsAny<Author>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldCallRepositoryDeleteAndSave()
        {
            // Arrange
            var authorId = 1;
            var author = new Author { Id = authorId, Name = "Author 1" };
            var authors = new List<Author> { author };

            _mockAuthorRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Author, bool>>>()))
                .ReturnsAsync(authors);

            _mockAuthorRepository
                .Setup(x => x.Delete(author))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _authorService.Delete(authorId);

            // Assert
            _mockAuthorRepository.Verify(
                x => x.FindByCondition(It.IsAny<Expression<Func<Author, bool>>>()),
                Times.Once);
            _mockAuthorRepository.Verify(x => x.Delete(author), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var nonExistingId = 999;
            var emptyList = new List<Author>();

            _mockAuthorRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Author, bool>>>()))
                .ReturnsAsync(emptyList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _authorService.Delete(nonExistingId));

            _mockAuthorRepository.Verify(x => x.Delete(It.IsAny<Author>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _authorService.Delete(invalidId));

            _mockAuthorRepository.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Author, bool>>>()), Times.Never);
            _mockAuthorRepository.Verify(x => x.Delete(It.IsAny<Author>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenMultipleRecordsFound_ShouldDeleteFirstOne()
        {
            // Arrange
            var authorId = 1;
            var authors = new List<Author>
            {
                new Author { Id = authorId, Name = "First" },
                new Author { Id = authorId, Name = "Second" } // Same ID - unusual but possible
            };

            _mockAuthorRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Author, bool>>>()))
                .ReturnsAsync(authors);

            _mockAuthorRepository
                .Setup(x => x.Delete(It.IsAny<Author>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _authorService.Delete(authorId);

            // Assert
            _mockAuthorRepository.Verify(x => x.Delete(authors.First()), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public void Constructor_WithNullRepositoryWrapper_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AuthorService(null));
        }

        [Fact]
        public void Constructor_WithValidRepositoryWrapper_ShouldInitializeService()
        {
            // Arrange
            var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            // Act
            var service = new AuthorService(mockRepositoryWrapper.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task Delete_WhenFindByConditionReturnsNull_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var authorId = 1;
            List<Author> nullList = null;

            _mockAuthorRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Author, bool>>>()))
                .ReturnsAsync(nullList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _authorService.Delete(authorId));

            _mockAuthorRepository.Verify(x => x.Delete(It.IsAny<Author>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }
    }
}