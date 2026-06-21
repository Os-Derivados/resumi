using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resumi.Domain.Exceptions;

namespace Resumi.Domain.Models;

/// <summary>
/// Este contrato representa um item de uma seção dentro de um <see cref="Resume"/>.
/// </summary>
public abstract class ResumeNode : Entity
{
	public static readonly string NameTooLong = "O nome do item não pode exceder 128 caracteres.";
	public static readonly string DescriptionTooLong = "A descrição do item não pode exceder 256 caracteres.";

	public static readonly string InstitutionNameTooLong =
		"O nome da instituição associada ao item não pode exceder 128 caracteres.";

	public static readonly string InvalidEngagement =
		"O voluntariado não pode ser marcado como 'ainda engajado' se a data de término estiver preenchida, e vice-versa.";

	[Required]
	[ForeignKey(nameof(Resume))]
	public required int ResumeId { get; set; }

	public Resume? Resume { get; set; }

	[Required] [StringLength(128)] public required string Name { get; set; }

	[Required] [StringLength(256)] public required string Description { get; set; }

	/// <summary>
	/// Representa o nome da empresa, instituição de ensino ou ONG associada a <see cref="Experience"/>,
	/// <see cref="Degree"/> ou <see cref="Volunteership"/>, respectivamente.
	/// </summary>
	[Required]
	[StringLength(128)]
	public required string InstitutionName { get; set; }

	[StringLength(64)] public string? Location { get; set; }

	[DefaultValue(false)] [Required] public bool IsRemote { get; set; }

	[Required] public required DateTime StartDate { get; set; }

	private DateTime? _endDate;

	public DateTime? EndDate
	{
		get => _endDate;
		set
		{
			if (value is not null && StillEngaged)
			{
				throw new StillEngagedException();
			}

			_endDate = value;
		}
	}

	private bool _stillEngaged;

	[Required]
	public bool StillEngaged
	{
		get => _stillEngaged;
		set
		{
			if (value && EndDate is not null)
			{
				throw new StillEngagedException();
			}

			_stillEngaged = value;
		}
	}
}
