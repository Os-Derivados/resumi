using System.ComponentModel.DataAnnotations;

namespace Resumi.Domain.Models;

public abstract class Entity : ITrackable
{
    public static readonly string UpdatePrimaryKeyMismatch =
        "A chave da entidade atual e da nova entidade devem ser iguais.";

    [Key] public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
