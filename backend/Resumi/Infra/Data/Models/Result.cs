namespace Resumi.Infra.Data.Models;

/// <summary>
/// Encpasula o resultado de uma operação, incluindo sucesso ou falha e mensagens associadas.
/// </summary>
/// <typeparam name="T">Tipo do valor retornado em caso de sucesso.</typeparam>
public class Result<T>
{
    public bool Succeeded { get; init; }
    public T? Data { get; init; }
    public List<string> AllErrors => Errors.Values.SelectMany(e => e).Distinct().ToList();
    public ResultDictionary Errors { get; init; } = new();

    /// <summary>
    /// Cria um resultado de sucesso contendo os dados fornecidos pela operação realizada.
    /// </summary>
    /// <param name="data">
    /// Os dados resultantes da operação bem-sucedida.
    /// </param>
    /// <returns>
    /// Uma instância de <see cref="Result{T}"/> representando o sucesso da operação com os dados fornecidos.
    /// </returns>
    public static Result<T> Success(T data) => new() { Succeeded = true, Data = data };

    public static Result<T> Failure(ResultDictionary errors) =>
        new() { Succeeded = false, Errors = errors };

    public static Result<T> Failure(string key, string errorMessage)
    {
        ResultDictionary errors = [];
        errors.AddError(key, errorMessage);

        return Failure(errors);
    }
}
