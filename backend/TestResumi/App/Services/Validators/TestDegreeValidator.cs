using Resumi.App.Services.Validators;

namespace TestResumi.App.Services.Validators;

public class TestDegreeValidator
{
    private readonly DegreeValidator _validator = new();

    [Fact]
    public void ValidateCreation_WithValidDegree_ReturnsSuccess()
    {
        // Arrange
        var degree = new Degree
        {
            Name = "Engenharia de Software",
            Description = "Curso de computação",
            InstitutionName = "UFRJ",
            Location = "Rio de Janeiro",
            IsRemote = false,
            StartDate = DateTime.Parse("2020-01-01"),
            EndDate = DateTime.Parse("2024-01-01"),
            StillEngaged = false,
            Highlights = "Destaques do curso",
            Level = DegreeLevel.Bachelor
        };

        // Act
        var result = _validator.ValidateCreation(degree);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void ValidateCreation_WithNullDegree_ReturnsFail()
    {
        // Act
        var result = _validator.ValidateCreation(null);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains("Estado inválido", result.Errors.SelectMany(x => x.Value).First());
    }

    [Fact]
    public void ValidateCreation_WithMissingName_ReturnsFail()
    {
        // Arrange
        var degree = new Degree
        {
            Name = null,
            Description = "Curso de computação",
            InstitutionName = "UFRJ",
            IsRemote = false,
            StartDate = DateTime.Now,
            StillEngaged = true,
            Level = DegreeLevel.Bachelor
        };

        // Act
        var result = _validator.ValidateCreation(degree);

        // Assert
        Assert.False(result.Succeeded);
    }

    [Fact]
    public void ValidateCreation_WithEndDateBeforeStartDate_ReturnsFail()
    {
        // Arrange
        var degree = new Degree
        {
            Name = "Engenharia",
            Description = "Curso",
            InstitutionName = "UFRJ",
            IsRemote = false,
            StartDate = DateTime.Parse("2024-01-01"),
            EndDate = DateTime.Parse("2020-01-01"),
            StillEngaged = false,
            Level = DegreeLevel.Bachelor
        };

        // Act
        var result = _validator.ValidateCreation(degree);

        // Assert
        Assert.False(result.Succeeded);
    }

    [Fact]
    public void ValidateCreation_WithNameExceedingMaxLength_ReturnsFail()
    {
        // Arrange
        var degree = new Degree
        {
            Name = new string('a', 129), // Exceeds 128
            Description = "Curso",
            InstitutionName = "UFRJ",
            IsRemote = false,
            StartDate = DateTime.Now,
            StillEngaged = true,
            Level = DegreeLevel.Bachelor
        };

        // Act
        var result = _validator.ValidateCreation(degree);

        // Assert
        Assert.False(result.Succeeded);
    }

    [Fact]
    public void ValidateUpdate_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var currentDegree = new Degree
        {
            Id = 1,
            Name = "Engenharia",
            Description = "Curso",
            InstitutionName = "UFRJ",
            IsRemote = false,
            StartDate = DateTime.Parse("2020-01-01"),
            Level = DegreeLevel.Bachelor
        };

        var updatedDegree = new Degree
        {
            Id = 1,
            Name = "Engenharia de Software",
            Description = "Curso de computação",
            InstitutionName = "UFRJ",
            IsRemote = false,
            StartDate = DateTime.Parse("2020-01-01"),
            Level = DegreeLevel.Bachelor
        };

        // Act
        var result = _validator.ValidateUpdate(currentDegree, updatedDegree);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void ValidateUpdate_WithMismatchedIds_ReturnsFail()
    {
        // Arrange
        var currentDegree = new Degree
        {
            Id = 1, Name = "Eng", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false,
            StartDate = DateTime.Now, Level = DegreeLevel.Bachelor
        };
        var updatedDegree = new Degree
        {
            Id = 2, Name = "Eng", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false,
            StartDate = DateTime.Now, Level = DegreeLevel.Bachelor
        };

        // Act
        var result = _validator.ValidateUpdate(currentDegree, updatedDegree);

        // Assert
        Assert.False(result.Succeeded);
    }

    [Fact]
    public void ValidateDeletion_WithValidId_ReturnsSuccess()
    {
        // Arrange
        var degree = new Degree
        {
            Id = 1, Name = "Eng", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false,
            StartDate = DateTime.Now, Level = DegreeLevel.Bachelor
        };

        // Act
        var result = _validator.ValidateDeletion(degree);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void ValidateDeletion_WithInvalidId_ReturnsFail()
    {
        // Arrange
        var degree = new Degree
        {
            Id = 0, Name = "Eng", Description = "Desc", InstitutionName = "UFRJ", IsRemote = false,
            StartDate = DateTime.Now, Level = DegreeLevel.Bachelor
        };

        // Act
        var result = _validator.ValidateDeletion(degree);

        // Assert
        Assert.False(result.Succeeded);
    }
}
