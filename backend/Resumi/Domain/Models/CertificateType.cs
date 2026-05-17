namespace Resumi.Domain.Models;

/// <summary>
/// Representa os tipos de certificados que podem ser associados a um <see cref="Certificate"/>.
/// </summary>
public enum CertificateType
{
	/// <summary>
	/// Curso livre.
	/// </summary>
	Course = 0,

	/// <summary>
	/// Credencial fornecida por entidade certificadora.
	/// </summary>
	License = 1,

	/// <summary>
	/// Insígnia ou distintivo digital fornecido por entidade certificadora.
	/// </summary>
	Badge = 2,

	/// <summary>
	/// Certificado relacionado a atividades extracurriculares, eventos, ou projetos voluntários.
	/// </summary>
	Extracurricular = 3,

	/// <summary>
	/// Indicação ou nomeação por um prêmio ou reconhecimento.
	/// </summary>
	Nomination = 4,
}
