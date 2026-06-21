namespace Resumi.Domain.Exceptions;

/// <summary>
/// Define que um item marcado como "ainda em andamento" não pode ter uma data de término definida.
/// </summary>
public class StillEngagedException()
    : DomainException("A data de fim não pode ser definida quando o item ainda está em andamento.");