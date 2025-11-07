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
    public class RoleServiceTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IRoleRepository> _mockRoleRepository;
        private readonly RoleService _roleService;

        public RoleServiceTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockRoleRepository = new Mock<IRoleRepository>();

            _mockRepositoryWrapper
                .Setup(x => x.Role)
                .Returns(_mockRoleRepository.Object);

            _roleService = new RoleService(_mockRepositoryWrapper.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllRoles()
        {
            // Arrange
            var expectedRoles = new List<Role>
            {
                new Role { Id = 1, RoleName = "Admin" },
                new Role { Id = 2, RoleName = "User" },
                new Role { Id = 3, RoleName = "Moderator" }
            };

            _mockRoleRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(expectedRoles);

            // Act
            var result = await _roleService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(expectedRoles, result);
            _mockRoleRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var emptyList = new List<Role>();

            _mockRoleRepository
                .Setup(x => x.FindAll())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _roleService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRoleRepository.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            _mockRoleRepository
                .Setup(x => x.FindAll())
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _roleService.GetAll());
        }

        [Fact]
        public async Task Create_ShouldCallRepositoryCreateAndSave()
        {
            // Arrange
            var role = new Role { Id = 1, RoleName = "Editor" };

            _mockRoleRepository
                .Setup(x => x.Create(role))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _roleService.Create(role);

            // Assert
            _mockRoleRepository.Verify(x => x.Create(role), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Create_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _roleService.Create(null));

            _mockRoleRepository.Verify(x => x.Create(It.IsAny<Role>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Create_WhenSaveFails_ShouldPropagateException()
        {
            // Arrange
            var role = new Role { Id = 1, RoleName = "Editor" };

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .ThrowsAsync(new Exception("Save failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _roleService.Create(role));

            _mockRoleRepository.Verify(x => x.Create(role), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryUpdateAndSave()
        {
            // Arrange
            var role = new Role { Id = 1, RoleName = "Updated Role" };

            _mockRoleRepository
                .Setup(x => x.Update(role))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _roleService.Update(role);

            // Assert
            _mockRoleRepository.Verify(x => x.Update(role), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Update_WithNullModel_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _roleService.Update(null));

            _mockRoleRepository.Verify(x => x.Update(It.IsAny<Role>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldCallRepositoryDeleteAndSave()
        {
            // Arrange
            var roleId = 1;
            var role = new Role { Id = roleId, RoleName = "Admin" };
            var roles = new List<Role> { role };

            _mockRoleRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>()))
                .ReturnsAsync(roles);

            _mockRoleRepository
                .Setup(x => x.Delete(role))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _roleService.Delete(roleId);

            // Assert
            _mockRoleRepository.Verify(
                x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>()),
                Times.Once);
            _mockRoleRepository.Verify(x => x.Delete(role), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var nonExistingId = 999;
            var emptyList = new List<Role>();

            _mockRoleRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>()))
                .ReturnsAsync(emptyList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _roleService.Delete(nonExistingId));

            _mockRoleRepository.Verify(x => x.Delete(It.IsAny<Role>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidId = 0;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _roleService.Delete(invalidId));

            _mockRoleRepository.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>()), Times.Never);
            _mockRoleRepository.Verify(x => x.Delete(It.IsAny<Role>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenMultipleRecordsFound_ShouldDeleteFirstOne()
        {
            // Arrange
            var roleId = 1;
            var roles = new List<Role>
            {
                new Role { Id = roleId, RoleName = "First" },
                new Role { Id = roleId, RoleName = "Second" }
            };

            _mockRoleRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>()))
                .ReturnsAsync(roles);

            _mockRoleRepository
                .Setup(x => x.Delete(It.IsAny<Role>()))
                .Returns(Task.CompletedTask);

            _mockRepositoryWrapper
                .Setup(x => x.Save())
                .Returns(Task.CompletedTask);

            // Act
            await _roleService.Delete(roleId);

            // Assert
            _mockRoleRepository.Verify(x => x.Delete(roles.First()), Times.Once);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public void Constructor_WithNullRepositoryWrapper_ShouldThrowArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new RoleService(null));
        }

        [Fact]
        public void Constructor_WithValidRepositoryWrapper_ShouldInitializeService()
        {
            // Arrange
            var mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            // Act
            var service = new RoleService(mockRepositoryWrapper.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task Delete_WhenFindByConditionReturnsNull_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var roleId = 1;
            List<Role> nullList = null;

            _mockRoleRepository
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<Role, bool>>>()))
                .ReturnsAsync(nullList);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _roleService.Delete(roleId));

            _mockRoleRepository.Verify(x => x.Delete(It.IsAny<Role>()), Times.Never);
            _mockRepositoryWrapper.Verify(x => x.Save(), Times.Never);
        }
    }
}