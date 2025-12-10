namespace Resumi.Infra.Exceptions;

/// <summary>
/// Representa uma violação de regra de negócio na camada de infraestrutura da aplicação.
/// </summary>
/// <param name="message">Uma mensagem descrevendo a regra violada.</param>
public class InfrastructureException(string message) : Exception(message);
