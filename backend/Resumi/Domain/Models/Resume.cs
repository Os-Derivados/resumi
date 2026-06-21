using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.Domain.Models;

/// <summary>
/// Representa um currículo dentro da aplicação, a nível de banco de dados.
/// </summary>
[Table(("Resumes"))]
public class Resume : Entity
{
    public static readonly string FailedToQuery = "Não foi possível buscar o Currículo.";
    public static readonly string FailedToCreate = "Não foi pssível cadastrar o Currículo.";
    public static readonly string FailedToUpdate = "Não foi possível atualizar o Currículo.";
    public static readonly string FailedToDelete = "Não foi possível excluir o Currículo.";

    [Required] [ForeignKey(nameof(User))] public int UserId { get; set; }
    public AppUser? User { get; set; }

    /// <summary>
    /// Representa o título do currículo enquanto arquivo, para metadado.
    /// </summary>
    [Required]
    [StringLength(128)]
    public required string Title { get; set; }

    /// <summary>
    /// Representa o nome do proprietário do currículo, para exibição.
    /// </summary>
    [Required]
    [StringLength(128)]
    public string? OwnerName { get; set; }

    public string? Location { get; set; }

    [Required] [EmailAddress] public string? Email { get; set; }

    [Required] [Phone] public string? PhoneNumber { get; set; }

    /// <summary>
    /// Representa palavras-chave associadas ao currículo, para facilitar buscas.
    /// </summary>
    public string? Keywords { get; set; }

    public ICollection<Experience>? Experiences { get; set; }
    public ICollection<Degree>? AcademicDegrees { get; set; }
    public ICollection<Volunteership>? VolunteerExperiences { get; set; }
    public ICollection<Certificate>? Certificates { get; set; }

    public override Resume? ShallowCopy()
    {
        try
        {
            return new Resume
            {
                Id = Id,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                UserId = UserId,
                Title = Title,
                OwnerName = OwnerName,
                Location = Location,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Keywords = Keywords
            };
        }
        catch (InvalidCastException)
        {
            return null;
        }
    }
}
