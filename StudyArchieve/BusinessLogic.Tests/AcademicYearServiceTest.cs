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
    public class AcademicYearServiceTest
    {
        private readonly AcademicYearService _service;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMoq;
        private readonly Mock<IAcademicYearRepository> _academicYearRepositoryMoq;

        public AcademicYearServiceTest()
        {
            _repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            _academicYearRepositoryMoq = new Mock<IAcademicYearRepository>();

            _repositoryWrapperMoq.Setup(x => x.AcademicYear)
                                .Returns(_academicYearRepositoryMoq.Object);

            _service = new AcademicYearService(_repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllAcademicYears()
        {
            // Arrange
            var academicYears = new List<AcademicYear>
        {
            new AcademicYear { Id = 1, YearLabel = "2020-2021" },
            new AcademicYear { Id = 2, YearLabel = "2021-2022" },
            new AcademicYear { Id = 3, YearLabel = "2022-2023" }
        };

            _academicYearRepositoryMoq.Setup(x => x.FindAll())
                                     .ReturnsAsync(academicYears);

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            Assert.Equal(1, result[0].Id);
            Assert.Equal("2020-2021", result[0].YearLabel);

            Assert.Equal(2, result[1].Id);
            Assert.Equal("2021-2022", result[1].YearLabel);

            Assert.Equal(3, result[2].Id);
            Assert.Equal("2022-2023", result[2].YearLabel);

            _academicYearRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_EmptyRepository_ReturnsEmptyList()
        {
            // Arrange
            _academicYearRepositoryMoq.Setup(x => x.FindAll())
                                     .ReturnsAsync(new List<AcademicYear>());

            // Act
            var result = await _service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _academicYearRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_RepositoryThrowsException_ThrowsSameException()
        {
            // Arrange
            var expectedException = new Exception("Database error");
            _academicYearRepositoryMoq.Setup(x => x.FindAll())
                                     .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAll());
            Assert.Equal("Database error", exception.Message);

            _academicYearRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }
    }
}
