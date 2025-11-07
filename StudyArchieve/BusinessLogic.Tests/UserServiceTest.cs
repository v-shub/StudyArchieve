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
    public class UserServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.User)
                .Returns(_mockUserRepository.Object);

            _userService = new UserService(_mockRepositoryWrapper.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllUsers()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { Id = 1, Username = "user1", Email = "user1@test.com", Password = "pass1", RoleId = 1, Role = new Role { Id = 1, RoleName = "User" } },
                new User { Id = 2, Username = "user2", Email = "user2@test.com", Password = "pass2", RoleId = 2, Role = new Role { Id = 2, RoleName = "Admin" } }
            };

            _mockUserRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await _userService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedUsers, result);
            _mockUserRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyList = new List<User>();

            _mockUserRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _userService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockUserRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            _mockUserRepository
                .Setup(x => x.FindAll())
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.GetAll());
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User { Id = userId, Username = "user1", Email = "user1@test.com", Password = "pass1", RoleId = 1, Role = new Role { Id = 1, RoleName = "User" } };

            _mockUserRepository
                .Setup(x => x.GetById(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser, result);
            Assert.NotNull(result.Role);
            _mockUserRepository.Verify(x => x.GetById(userId), Times.Once);
        }

        [Fact]
        public async Task GetById_WithNonExistingId_ShouldReturnNull()
        {
            // Arrange
            var nonExistingId = 999;

            _mockUserRepository
                .Setup(x => x.GetById(nonExistingId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetById(nonExistingId);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(x => x.GetById(nonExistingId), Times.Once);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.GetById(invalidId));

            _mockUserRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldCallRepositoryCreateAndSave()
        {
            // Arrange
            var user = new User { Id = 1, Username = "newuser", Email = "new@test.com", Password = "password", RoleId = 1 };

            _mockUserRepository
                .Setup(x => x.Create(user))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _userService.Create(user);

            // Assert
            _mockUserRepository.Verify(x => x.Create(user), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.Create(null));

            _mockUserRepository.Verify(x => x.Create(It.IsAny<User>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_WhenSaveFails_ShouldPropagateException()
        {
            // Arrange
            var user = new User { Id = 1, Username = "newuser", Email = "new@test.com", Password = "password", RoleId = 1 };

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .ThrowsAsync(new Exception("Save failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _userService.Create(user));

            _mockUserRepository.Verify(x => x.Create(user), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdateAndSave()
        {
            // Arrange
            var user = new User { Id = 1, Username = "updateduser", Email = "updated@test.com", Password = "newpass", RoleId = 2 };

            _mockUserRepository
                .Setup(x => x.Update(user))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _userService.Update(user);

            // Assert
            _mockUserRepository.Verify(x => x.Update(user), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.Update(null));

            _mockUserRepository.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldCallRepositoryDeleteAndSave()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Username = "user1", Email = "user1@test.com", Password = "pass1", RoleId = 1 };
            var users = new List<User> { user };

            _mockUserRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(users);

            _mockUserRepository
                .Setup(x => x.Delete(user))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _userService.Delete(userId);

            // Assert
            _mockUserRepository.Verify(
                x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()),
                Times.Once);
            _mockUserRepository.Verify(x => x.Delete(user), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var nonExistingId = 999;
            var emptyList = new List<User>();

            _mockUserRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(emptyList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.Delete(nonExistingId));

            _mockUserRepository.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.Delete(invalidId));

            _mockUserRepository.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()), Times.Never);
            _mockUserRepository.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenMultipleRecordsFound_ShouldDeleteFirstOne()
        {
            // Arrange
            var userId = 1;
            var users = new List<User>
            {
                new User { Id = userId, Username = "First", Email = "first@test.com", Password = "pass1", RoleId = 1 },
                new User { Id = userId, Username = "Second", Email = "second@test.com", Password = "pass2", RoleId = 1 }
            };

            _mockUserRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(users);

            _mockUserRepository
                .Setup(x => x.Delete(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _userService.Delete(userId);

            // Assert
            _mockUserRepository.Verify(x => x.Delete(users.First()), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenFindByConditionReturnsNull_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var userId = 1;
            List<User> nullList = null;

            _mockUserRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(nullList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.Delete(userId));

            _mockUserRepository.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public void Constructor_WithNullRepositoryWrapper_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new UserService(null));
        }

        [Fact]
        public void Constructor_WithValidRepositoryWrapper_ShouldInitializeService()
        {
            // Arrange
            var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            // Act
            var service = new UserService(mockRepositoryWrapper.Object);

            // Assert
            Assert.NotNull(service);
        }
    }
}