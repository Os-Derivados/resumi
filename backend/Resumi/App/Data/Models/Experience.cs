using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.App.Data.Models;

[Table(("Experiences"))]
public class Experience : ResumeNode
{
    public string? Highlights { get; set; }
}
