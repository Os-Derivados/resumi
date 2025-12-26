namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros para a leitura
/// de um <see cref="Resume"/> fora do domínio da aplicação.
/// </summary>
public record ResumeModel
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string OwnerName { get; init; }
    public string? Location { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public string? Keyword { get; init; }
}
