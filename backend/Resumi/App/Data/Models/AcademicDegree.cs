using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.App.Data.Models;

/// <summary>
/// Representa uma formação acadêmica dentro de um <see cref="Resume"/>.
/// </summary>
[Table("AcademicDegrees")]
public class AcademicDegree : ResumeNode
{
    public string? Highlights { get; set; }

    [Required]
    public DegreeLevel Level { get; set; }
}
