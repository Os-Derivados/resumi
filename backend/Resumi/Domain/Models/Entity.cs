using System.ComponentModel.DataAnnotations;

namespace Resumi.Domain.Models;

public abstract class Entity : ITrackable, ICloneableEntity<Entity>
{
    public static readonly string UpdatePrimaryKeyMismatch =
        "A chave da entidade atual e da nova entidade devem ser iguais.";

    public static readonly string CannotUpdateFromSameEntity = "A entidade atualizada deve ser uma cópia da original";

    public static readonly string InvalidPrimaryKey = "A chave primária da entidade é inválida.";

    public static readonly string InvalidState = "O estado da entidade é inválido.";

    public static readonly string NotFound = "O registro não foi encontrado.";

    [Key] public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public abstract Entity? ShallowCopy();
}
