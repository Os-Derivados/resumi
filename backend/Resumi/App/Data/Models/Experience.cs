using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.App.Data.Models;

/// <summary>
/// Representa uma experiência profissional dentro de um <see cref="Resume"/>.
/// </summary>
[Table(("Experiences"))]
public class Experience : ResumeNode
{
    public string? Highlights { get; set; }
}
