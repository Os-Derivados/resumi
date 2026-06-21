using System.ComponentModel.DataAnnotations.Schema;

namespace Resumi.Domain.Models;

/// <summary>
/// Representa uma experiência profissional dentro de um <see cref="Resume"/>.
/// </summary>
[Table("Experiences")]
public class Experience : ResumeNode
{
	public static readonly string FailedToCreate = "Não foi possível cadastrar experiência profissional.";
	public static readonly string FailedToUpdate = "Não foi possível atualizar a experiência profissional.";
	public static readonly string FailedToDelete = "Não foi possível deletar a experiência profissional.";
	public static readonly string InternalError = "Ocorreu um erro interno ao processar experiência profissional.";

	public string? Highlights { get; set; }

	public override Experience ShallowCopy()
	{
		return new Experience
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

			#region Campos de Experience

			Highlights = Highlights,

			#endregion
		};
	}
}
