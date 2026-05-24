using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Resumi.Domain.Models;

public class AppUser : IdentityUser<int>, ITrackable
{
    public static readonly string NotFound = "Usuário não encontrado.";
    public static readonly string FailedToCreate = "Falha ao criar usuário.";
    public static readonly string FailedToUpdate = "Falha ao atualizar usuário.";
    public static readonly string FailedToDelete = "Falha ao excluir usuário.";
    public static readonly string InvalidState = "O Usuário encontra-se em um estado inválido para esta operação.";
    public static readonly string InternalError = "Ocorreu um erro interno ao processar a solicitação.";

    [Required] [StringLength(128)] public required string FullName { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Resume> Resumes { get; set; } = [];
}
