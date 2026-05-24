using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.Domain.Models;

/// <summary>
/// Representa uma experiência de voluntariado dentro de um <see cref="Resume"/>.
/// </summary>
[Table("Volunteerships")]
public class Volunteership : ResumeNode
{
	public override Volunteership ShallowCopy()
	{
		return new Volunteership
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
		};
	}
}
