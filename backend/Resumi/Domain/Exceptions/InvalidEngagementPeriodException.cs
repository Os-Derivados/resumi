using Resumi.Domain.Models;

namespace Resumi.Domain.Exceptions;

/// <summary>
/// Define que a data de término deve ser posterior à data de início para um período de atividade de um <see cref="ResumeNode"/>.
/// </summary>
public class InvalidEngagementPeriodException() : DomainException("A data de término deve ser posterior à data de início para o período de engajamento.");