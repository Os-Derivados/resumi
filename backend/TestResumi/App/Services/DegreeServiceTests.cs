using Microsoft.EntityFrameworkCore;
using Moq;
using Resumi.App.Data.Models;
using Resumi.App.Services;
using Resumi.App.Services.Interfaces;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Database.Interfaces;

namespace TestResumi.App.Services;

public class DegreeServiceTests
{
    private readonly Mock<IDomainValidator<Degree>> _mockValidator;
    private readonly Mock<IRepository<Degree>> _mockRepository;
    private readonly AppDbContext _dbContext;
    private readonly DegreeService _degreeService;

    public DegreeServiceTests()
    {
        _mockValidator = new Mock<IDomainValidator<Degree>>();
        _mockRepository = new Mock<IRepository<Degree>>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"DegreeServiceTests_{Guid.NewGuid()}")
            .Options;

        _dbContext = new AppDbContext(options);

        _degreeService = new DegreeService(_mockValidator.Object, _mockRepository.Object, _dbContext);
    }

    [Fact]
    public async Task CreateAsync_WithValidDegree_ReturnsSuccess()
    {
        // Arrange
        var degree = new Degree
        {
            Name = "Engenharia",
            Description = "Curso",
            InstitutionName = "UFRJ",
            IsRemote = false,
            StartDate = DateTime.Now,
            StillEngaged = true,
            Level = DegreeLevel.Bachelor
        };

        _mockValidator
            .Setup(v => v.ValidateCreation(It.IsAny<Degree>()))
            .Returns(Result<Degree>.Success(degree));

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Degree>()))
            .ReturnsAsync(degree);

        // Act
        var result = await _degreeService.CreateAsync(degree);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(degree.Id, result.Data?.Id);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Degree>()), Times.Once);

    }

    [Fact]
    public async Task CreateAsync_WithInvalidDegree_ReturnsFail()
    {
        // Arrange
        var degree = new Degree();
        var errors = new ResultDictionary { { "Name", new List<string> { "Nome é obrigatório" } } };

        _mockValidator
            .Setup(v => v.ValidateCreation(It.IsAny<Degree>()))
            .Returns(Result<Degree>.Failure(errors));

        // Act
        var result = await _degreeService.CreateAsync(degree);

        // Assert
        Assert.False(result.Succeeded);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Degree>()), Times.Never);
    }

    [Fact]
    public async Task FindAsync_WithValidId_ReturnsSuccess()
    {
        // Arrange
        var degreeId = 1;
        var degree = new Degree { Id = degreeId, Name = "Eng", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false, StartDate = DateTime.Now, Level = DegreeLevel.Bachelor };

        _mockRepository
            .Setup(r => r.GetByIdAsync(degreeId))
            .ReturnsAsync(degree);

        // Act
        var result = await _degreeService.FindAsync(degreeId);

        // Assert
        Assert.True(result.Succeeded);
        Assert.Equal(degreeId, result.Data?.Id);
    }

    [Fact]
    public async Task FindAsync_WithInvalidId_ReturnsFail()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Degree?)null);

        // Act
        var result = await _degreeService.FindAsync(0);

        // Assert
        Assert.False(result.Succeeded);
    }

    [Fact]
    public async Task FindAllAsync_WithValidParameters_ReturnsSuccess()
    {
        // Arrange
        var degrees = new List<Degree>
        {
            new Degree { Id = 1, Name = "Eng1", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false, StartDate = DateTime.Now, Level = DegreeLevel.Bachelor },
            new Degree { Id = 2, Name = "Eng2", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false, StartDate = DateTime.Now, Level = DegreeLevel.Bachelor }
        };

        _mockRepository
            .Setup(r => r.GetAllAsync(0, 20))
            .ReturnsAsync(degrees);

        // Act
        var result = await _degreeService.FindAllAsync(0, 20);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var currentDegree = new Degree { Id = 1, Name = "Eng", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false, StartDate = DateTime.Now, Level = DegreeLevel.Bachelor };
        var updatedDegree = new Degree { Id = 1, Name = "Updated", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false, StartDate = DateTime.Now, Level = DegreeLevel.Bachelor };

        _mockValidator
            .Setup(v => v.ValidateUpdate(It.IsAny<Degree>(), It.IsAny<Degree>()))
            .Returns(Result<Degree>.Success(updatedDegree));

        _mockRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Degree>()))
            .ReturnsAsync(updatedDegree);

        // Act
        var result = await _degreeService.UpdateAsync(currentDegree, updatedDegree);

        // Assert
        Assert.True(result.Succeeded);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Degree>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsSuccess()
    {
        // Arrange
        var degreeId = 1;

        _mockRepository
            .Setup(r => r.DeleteAsync(degreeId))
            .ReturnsAsync(true);

        // Act
        var result = await _degreeService.DeleteAsync(degreeId);

        // Assert
        Assert.True(result.Succeeded);
        _mockRepository.Verify(r => r.DeleteAsync(degreeId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsFail()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _degreeService.DeleteAsync(0);

        // Assert
        Assert.False(result.Succeeded);
    }
}
