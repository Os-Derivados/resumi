using Resumi.Domain.Exceptions;

namespace Resumi.Domain.Models;

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

public static class DegreeLevelExtensions
{
	public static string ToDisplayString(this DegreeLevel level)
	{
		return level switch
		{
			DegreeLevel.HighSchool => "high_school",
			DegreeLevel.Technical => "technical",
			DegreeLevel.Associate => "associate",
			DegreeLevel.Bachelor => "bachelor",
			DegreeLevel.Master => "master",
			DegreeLevel.Doctorate => "doctorate",
			DegreeLevel.Other => "other",
			_ => level.ToString()
		};
	}

	public static DegreeLevel FromDisplayString(string? level)
	{
		return level?.ToLower() switch
		{
			"high_school" => DegreeLevel.HighSchool,
			"technical" => DegreeLevel.Technical,
			"associate" => DegreeLevel.Associate,
			"bachelor" => DegreeLevel.Bachelor,
			"master" => DegreeLevel.Master,
			"doctorate" => DegreeLevel.Doctorate,
			"other" => DegreeLevel.Other,
			_ => throw new DomainException($"Invalid degree level: {level}")
		};
	}

	public static bool TryGetValue(string? level, out DegreeLevel? degreeLevel)
	{
		try
		{
			degreeLevel = FromDisplayString(level);

			return true;
		}
		catch (DomainException)
		{
			degreeLevel = null;

			return false;
		}
	}
}