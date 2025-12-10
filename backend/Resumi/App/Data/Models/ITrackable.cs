namespace Resumi.App.Data.Models;

/// <summary>
/// Este contrato define propriedades de rastreamento para entidades.
/// </summary>
public interface ITrackable
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
}
