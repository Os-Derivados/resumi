using Resumi.Domain.Exceptions;
using Resumi.Domain.Models;

namespace TestResumi.Domain.Models;

public class TestResumeNode
{
    [Fact]
    public void StillEngaged_WhenSetToFalse_ThrowsException_IfEndDateIsNull()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<StillEngagedException>(() => new Volunteership
            {
                ResumeId = 1,
                Name = "Test Node",
                Description = "Test Description",
                InstitutionName = "Test Institution",
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = DateTime.Now,
                StillEngaged = true,
            }
        );
    }

    [Fact]
    public void EndDate_WhenSetToNonNull_ThrowsException_IfStillEngagedIsTrue()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<StillEngagedException>(() => new Volunteership
            {
                ResumeId = 1,
                Name = "Test Node",
                Description = "Test Description",
                InstitutionName = "Test Institution",
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = DateTime.Now,
                StillEngaged = true,
            }
        );
    }
}