namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros para a leitura
/// de um <see cref="Resume"/> fora do domínio da aplicação.
/// </summary>
public record ResumeModel
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required string OwnerName { get; init; }
    public string? Location { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public string? Keyword { get; init; }
    public ExperienceModel[]? Experiences { get; init; }
    public DegreeModel[]? Degrees { get; init; }
    public VolunteershipModel[]? Volunteerships { get; init; }
}
