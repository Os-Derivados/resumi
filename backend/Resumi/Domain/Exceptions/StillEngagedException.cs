namespace Resumi.Domain.Exceptions;

public class StillEngagedException()
    : DomainException("A data de fim não pode ser definida quando o item ainda está em andamento.");