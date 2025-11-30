namespace Resumi.Api.Exceptions;

/// <summary>
/// Representa uma violação de regra de negócio na camada de API da aplicação.
/// </summary>
/// <param name="message">Uma mensagem descrevendo a regra violada.</param>
public class ApiException(string message) : Exception(message);
