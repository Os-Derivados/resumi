using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Resumi.App.Services;
using Resumi.Domain.Models;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;

namespace TestResumi.App.Services;

public class TestDegreeService
{
	private readonly Mock<IDomainValidator<Degree>> _mockValidator;
	private readonly Mock<ILogger<DegreeService>> _mockLogger = new();
	private readonly DegreeService _degreeService;

	public TestDegreeService()
	{
		_mockValidator = new Mock<IDomainValidator<Degree>>();

		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(databaseName: $"DegreeServiceTests_{Guid.NewGuid()}")
			.Options;

		var dbContext = new AppDbContext(options);

		_degreeService = new DegreeService(_mockValidator.Object, _mockLogger.Object, dbContext);
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

		// Act
		var result = await _degreeService.CreateAsync(degree);

		// Assert
		Assert.True(result.Succeeded);
		Assert.Equal(degree.Id, result.Data?.Id);
	}

	[Fact]
	public async Task CreateAsync_WithInvalidDegree_ReturnsFail()
	{
		// Arrange
		var degree = new Degree();
		var errors = new ResultDictionary { { "Name", ["Nome é obrigatório"] } };

		_mockValidator
			.Setup(v => v.ValidateCreation(It.IsAny<Degree>()))
			.Returns(Result<Degree>.Failure(errors));

		// Act
		var result = await _degreeService.CreateAsync(degree);

		// Assert
		Assert.False(result.Succeeded);
	}

	[Fact]
	public async Task UpdateAsync_WithValidData_ReturnsSuccess()
	{
		// Arrange
		Degree currentDegree = new()
		{
			Id = 1, Name = "Eng", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false,
			StartDate = DateTime.Now, Level = DegreeLevel.Bachelor
		};
		Degree updatedDegree = new()
		{
			Id = 1, Name = "Updated", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false,
			StartDate = DateTime.Now, Level = DegreeLevel.Bachelor
		};

		_mockValidator
			.Setup(v => v.ValidateUpdate(It.IsAny<Degree>(), It.IsAny<Degree>()))
			.Returns(Result<Degree>.Success(updatedDegree));

		// Act
		var result = await _degreeService.UpdateAsync(currentDegree, updatedDegree);

		// Assert
		Assert.True(result.Succeeded);
	}

	[Fact]
	public async Task DeleteAsync_WithValidId_ReturnsSuccess()
	{
		// Arrange
		const int degreeId = 1;

		// Act
		var result = await _degreeService.DeleteAsync(degreeId);

		// Assert
		Assert.True(result.Succeeded);
	}

	[Fact]
	public async Task DeleteAsync_WithInvalidId_ReturnsFail()
	{
		// Act
		var result = await _degreeService.DeleteAsync(0);

		// Assert
		Assert.False(result.Succeeded);
	}
}
