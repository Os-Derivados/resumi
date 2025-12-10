using System.ComponentModel.DataAnnotations;

namespace Resumi.App.Data.Models;

public abstract class Entity : ITrackable
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
