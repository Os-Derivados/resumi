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

	public DateTime? EndDate { get; set; }

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
