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
    public static readonly string NotFound = "Currículo não encontrado.";
    public static readonly string InvalidState = "O Currículo se encontra num estado inválido.";
    public static readonly string FailedToCreate = "Não foi pssível cadastrar o Currículo.";
    public static readonly string FailedToUpdate = "Não foi possível atualizar o Currículo.";

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

    [Required] public string? Email { get; set; }

    [Phone] [Required] public string? PhoneNumber { get; set; }

    /// <summary>
    /// Representa palavras-chave associadas ao currículo, para facilitar buscas.
    /// </summary>
    public string? Keywords { get; set; }

    public ICollection<Experience>? Experiences { get; set; }
    public ICollection<Degree>? AcademicDegrees { get; set; }
    public ICollection<Volunteership>? VolunteerExperiences { get; set; }
    public ICollection<Certificate>? Certificates { get; set; }

    public override Resume? ShallowCopy(Entity baseEntity)
    {
        try
        {
            var resume = (Resume)baseEntity;

            return new Resume
            {
                Id = resume.Id,
                CreatedAt = resume.CreatedAt,
                UpdatedAt = resume.UpdatedAt,
                UserId = resume.UserId,
                Title = resume.Title,
                OwnerName = resume.OwnerName,
                Location = resume.Location,
                Email = resume.Email,
                PhoneNumber = resume.PhoneNumber,
                Keywords = resume.Keywords
            };
        }
        catch (InvalidCastException)
        {
            return null;
        }
    }
}
