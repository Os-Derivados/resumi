using Resumi.App.Data.Models;

namespace Resumi.Api.Data.Models;

/// <summary>
/// Representa um objeto de parâmetros para a leitura
/// de um <see cref="Resume"/> fora do domínio da aplicação.
/// </summary>
public record ReadResumeModel(
    int Id,
    string Title,
    string OwnerName,
    string? Location,
    string Email,
    string PhoneNumber,
    string? Keywords
);
