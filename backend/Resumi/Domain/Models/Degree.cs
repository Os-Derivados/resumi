using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.Domain.Models;

/// <summary>
/// Representa uma formação acadêmica dentro de um <see cref="Resume"/>.
/// </summary>
[Table("Degrees")]
public class Degree : ResumeNode
{
	public static readonly string FailedToCreate = "Não foi possível cadastrar formação acadêmica.";

	public string? Highlights { get; set; }

	[Required] public DegreeLevel Level { get; set; }

	public override Degree ShallowCopy()
	{
		return new Degree
		{
			#region Campos de Entity

			Id = Id,
			CreatedAt = CreatedAt,
			UpdatedAt = UpdatedAt,

			#endregion

			#region Campos de ResumeNode

			ResumeId = ResumeId,
			Name = Name,
			Description = Description,
			InstitutionName = InstitutionName,
			Location = Location,
			IsRemote = IsRemote,
			StartDate = StartDate,
			EndDate = EndDate,
			StillEngaged = StillEngaged,

			#endregion

			#region Campos de Degree

			Highlights = Highlights,
			Level = Level

			#endregion
		};
	}
}
