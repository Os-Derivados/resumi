using System.ComponentModel.DataAnnotations;

namespace Resumi.Domain.Models;

public abstract class Entity : ITrackable, ICloneableEntity<Entity>
{
    public static readonly string UpdatePrimaryKeyMismatch =
        "A chave da entidade atual e da nova entidade devem ser iguais.";

    public static readonly string CannotUpdateFromSameEntity = "A entidade atualizada deve ser uma cópia da original";

    [Key] public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public abstract Entity? ShallowCopy(Entity baseEntity);
}
