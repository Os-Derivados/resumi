namespace Resumi.App.DTOs.Resume;

public class ResumeResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateResumeDto
{
    public string Title { get; set; }
    public string Summary { get; set; }
}

public class UpdateResumeDto
{
    public string Title { get; set; }
    public string Summary { get; set; }
}
