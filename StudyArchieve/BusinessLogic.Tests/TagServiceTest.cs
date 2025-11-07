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
    public class TagServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ITagRepository> _mockTagRepository;
        private readonly TagService _tagService;

        public TagServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockTagRepository = new Mock<ITagRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.Tag)
                .Returns(_mockTagRepository.Object);

            _tagService = new TagService(_mockRepositoryWrapper.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllTags()
        {
            // Arrange
            var expectedTags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Algebra" },
                new Tag { Id = 2, Name = "Geometry" }
            };

            _mockTagRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(expectedTags);

            // Act
            var result = await _tagService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedTags, result);
            _mockTagRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyList = new List<Tag>();

            _mockTagRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _tagService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockTagRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            _mockTagRepository
                .Setup(x => x.FindAll())
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _tagService.GetAll());
        }

        [Fact]
        public async Task Create_ShouldCallRepositoryCreateAndSave()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Trigonometry" };

            _mockTagRepository
                .Setup(x => x.Create(tag))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _tagService.Create(tag);

            // Assert
            _mockTagRepository.Verify(x => x.Create(tag), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tagService.Create(null));

            _mockTagRepository.Verify(x => x.Create(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_WhenSaveFails_ShouldNotCallCreate()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Trigonometry" };

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .ThrowsAsync(new Exception("Save failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _tagService.Create(tag));

            _mockTagRepository.Verify(x => x.Create(tag), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdateAndSave()
        {
            // Arrange
            var tag = new Tag { Id = 1, Name = "Updated Tag" };

            _mockTagRepository
                .Setup(x => x.Update(tag))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _tagService.Update(tag);

            // Assert
            _mockTagRepository.Verify(x => x.Update(tag), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tagService.Update(null));

            _mockTagRepository.Verify(x => x.Update(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldCallRepositoryDeleteAndSave()
        {
            // Arrange
            var tagId = 1;
            var tag = new Tag { Id = tagId, Name = "Algebra" };
            var tags = new List<Tag> { tag };

            _mockTagRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(tags);

            _mockTagRepository
                .Setup(x => x.Delete(tag))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _tagService.Delete(tagId);

            // Assert
            _mockTagRepository.Verify(
                x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()),
                Times.Once);
            _mockTagRepository.Verify(x => x.Delete(tag), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var nonExistingId = 999;
            var emptyList = new List<Tag>();

            _mockTagRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(emptyList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _tagService.Delete(nonExistingId));

            _mockTagRepository.Verify(x => x.Delete(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _tagService.Delete(invalidId));

            _mockTagRepository.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()), Times.Never);
            _mockTagRepository.Verify(x => x.Delete(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenMultipleRecordsFound_ShouldDeleteFirstOne()
        {
            // Arrange
            var tagId = 1;
            var tags = new List<Tag>
            {
                new Tag { Id = tagId, Name = "First" },
                new Tag { Id = tagId, Name = "Second" }
            };

            _mockTagRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(tags);

            _mockTagRepository
                .Setup(x => x.Delete(It.IsAny<Tag>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _tagService.Delete(tagId);

            // Assert
            _mockTagRepository.Verify(x => x.Delete(tags.First()), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public void Constructor_WithNullRepositoryWrapper_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TagService(null));
        }

        [Fact]
        public void Constructor_WithValidRepositoryWrapper_ShouldInitializeService()
        {
            // Arrange
            var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            // Act
            var service = new TagService(mockRepositoryWrapper.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task Delete_WhenFindByConditionReturnsNull_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var tagId = 1;
            List<Tag> nullList = null;

            _mockTagRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Tag, bool>>>()))
                .ReturnsAsync(nullList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _tagService.Delete(tagId));

            _mockTagRepository.Verify(x => x.Delete(It.IsAny<Tag>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }
    }
}