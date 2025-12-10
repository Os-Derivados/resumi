namespace Resumi.App.Data.Models;

/// <summary>
/// Representa o nível de um grau acadêmico na educação formal.
/// </summary>
public enum DegreeLevel
{
    /// <summary>
    /// Ensino Médio
    /// </summary>
    HighSchool,

    /// <summary>
    /// Ensino Técnico
    /// </summary>
    Technical,

    /// <summary>
    /// Ensino Superior Tecnólogo
    /// </summary>
    Associate,

    /// <summary>
    /// Ensino Superior Bacharelado
    /// </summary>
    Bachelor,

    /// <summary>
    /// Ensino Superior Mestrado
    /// </summary>
    Master,

    /// <summary>
    /// Ensino Superior Doutorado
    /// </summary>
    Doctorate,
    Other,
}
