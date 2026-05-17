using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.Domain.Models;

/// <summary>
/// Representa uma experiência de voluntariado dentro de um <see cref="Resume"/>.
/// </summary>
[Table("Volunteerships")]
public class Volunteership : ResumeNode;
