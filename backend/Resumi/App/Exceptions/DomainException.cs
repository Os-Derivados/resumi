namespace Resumi.App.Exceptions;

/// <summary>
/// Representa uma violação de regra de negócio na camada de aplicação.
/// </summary>
/// <param name="message">Uma mensagem descrevendo a regra violada.</param>
public class DomainException(string message) : Exception(message);
