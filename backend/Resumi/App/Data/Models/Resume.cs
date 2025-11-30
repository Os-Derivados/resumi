using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.App.Data.Models;

/// <summary>
/// Representa um currículo dentro da aplicação, a nível de banco de dados.
/// </summary>
[Table(("Resumes"))]
public class Resume : Entity
{
    /// <summary>
    /// Representa o título do currículo enquanto arquivo, para metadado.
    /// </summary>
    [Required]
    [StringLength(128)]
    public string? Title { get; set; }

    /// <summary>
    /// Representa o nome do proprietário do currículo, para exibição.
    /// </summary>
    [Required]
    [StringLength(128)]
    public string? OwnerName { get; set; }

    public string? Location { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? NormalizedEmail { get; set; }

    [Required]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Representa palavras-chave associadas ao currículo, para facilitar buscas.
    /// </summary>
    public string? Keywords { get; set; }

    public ICollection<Experience>? Experiences { get; set; }
    public ICollection<AcademicDegree>? AcademicDegrees { get; set; }
    public ICollection<VolunteerExperience>? VolunteerExperiences { get; set; }
}
