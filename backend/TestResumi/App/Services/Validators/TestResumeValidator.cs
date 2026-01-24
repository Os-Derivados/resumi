using Resumi.App.Data.Models;
using Resumi.App.Services.Validators;

namespace TestResumi.App.Services.Validators;

public class TestResumeValidator

{
    [Fact]
    public void CreationShouldFailIfTitleExceeds128Characters()
    {
        // Arrange
        const string titleOver128Chars = "This is a very long title that is definitely going to exceed the maximum allowed length of one hundred and twenty-eight characters. It just keeps going and going and going.";

        Resume newResume = new()
        {
            Title = titleOver128Chars
        };

        ResumeValidator validator = new();
        // Act
        var validationResult = validator.ValidateCreation(newResume);
        
        // Assert
        Assert.False(validationResult.Succeeded);
    }
}