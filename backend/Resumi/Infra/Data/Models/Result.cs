using System.Diagnostics.CodeAnalysis;
using Resumi.Infra.Exceptions;

namespace Resumi.Infra.Data.Models;

/// <summary>
/// Encapsula o resultado de uma operação, incluindo sucesso ou falha e mensagens associadas.
/// </summary>
public record Result
{
    protected Result(bool succeeded, ResultDictionary? errors)
    {
        if (!succeeded && errors is null)
        {
            throw new InfrastructureException("Failed operation must include errors");
        }

        Succeeded = succeeded;
        Errors = errors;
    }

    /// <summary>
    /// Indica se a operação foi bemsucedida.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Errors))]
    [MemberNotNullWhen(false, nameof(AllErrors))]
    public bool Succeeded { get; private init; }

    /// <summary>
    /// Contém a relação chave-valor dos possíveis errors ocorridos na operação.
    /// </summary>
    public ResultDictionary? Errors { get; }

    /// <summary>
    /// Representa a relação de erros <see cref="Errors"/> no formato de lista, sem as chaves de erro.
    /// </summary>
    public List<string>? AllErrors => Errors?.Values.SelectMany(e => e).Distinct().ToList();

    /// <summary>
    /// Indica o sucesso de uma operação.
    /// </summary>
    public static Result Success => new(true, null);

    /// <summary>
    /// Indica o fracasso da operação.
    /// </summary>
    /// <param name="result">Uma instância de <see cref="Result"/>, contendo o resultado de uma operação falha.</param>
    /// <returns>Uma instância de <see cref="Result"/>, indicando o fracasso da operação.</returns>
    public static Result Failure(Result result) => new(succeeded: false, result.Errors);

    /// <summary>
    /// Indica o fracasso da operação.
    /// </summary>
    /// <param name="errors">Uma instância de <see cref="ResultDictionary"/>, contendo a relação de erros da operação.</param>
    /// <returns>Uma instância de <see cref="Result"/>, indicando o fracasso da operação.</returns>
    public static Result Failure(ResultDictionary errors) => new(false, errors);

    /// <summary>
    /// Indica o fracasso da operação.
    /// </summary>
    /// <param name="errorKey">A chave de erro da operação falha.</param>
    /// <param name="errorMessage">A mensagem de erro da operação falha.</param>
    /// <returns>Uma instância de <see cref="Result"/>, indicando o fracasso da operação.</returns>
    public static Result Failure(string errorKey, string errorMessage)
    {
        ResultDictionary errors = [];

        errors.AddError(errorKey, errorMessage);

        return new Result(succeeded: false, errors);
    }
}

/// <summary>
/// Encapsula o resultado de uma operação, incluindo sucesso ou falha e mensagens associadas.
/// </summary>
/// <typeparam name="T">Tipo do valor retornado em caso de sucesso.</typeparam>
public record Result<T> : Result
{
    protected Result(bool succeeded, ResultDictionary? errors, T? data) : base(succeeded, errors)
    {
        if (succeeded && data is null)
        {
            throw new InfrastructureException("Succeeded operation must include data");
        }

        Data = data;
        Succeeded = succeeded;
    }

    [MemberNotNullWhen(true, nameof(Data))]
    public new bool Succeeded { get; private init; }

    /// <summary>
    /// Representa a informação resultante da operação.
    /// </summary>
    public T? Data { get; private init; }

    /// <summary>
    /// Indica o successo da operação.
    /// </summary>
    /// <param name="data">O valor retornado pela operação.</param>
    /// <returns>Uma instância de <see cref="Result"/>, indicando o sucesso da operação.</returns>
    public new static Result<T> Success(T data) => new(succeeded: true, errors: null, data);

    /// <summary>
    /// Indica o fracasso da operação.
    /// </summary>
    /// <param name="result">Uma instância de <see cref="Result"/>, contendo o resultado de uma operação falha.</param>
    /// <returns>Uma instância de <see cref="Result"/>, indicando o fracasso da operação.</returns>
    public new static Result<T> Failure(Result result) => new(succeeded: false, result.Errors, data: default);

    /// <summary>
    /// Indica o fracasso da operação.
    /// </summary>
    /// <param name="errors">Uma instância de <see cref="ResultDictionary"/>, contendo a relação de erros da operação.</param>
    /// <returns>Uma instância de <see cref="Result"/>, indicando o fracasso da operação.</returns>
    public new static Result<T> Failure(ResultDictionary errors) => new(false, errors, data: default);

    /// <summary>
    /// Indica o fracasso da operação.
    /// </summary>
    /// <param name="errorKey">A chave de erro da operação falha.</param>
    /// <param name="errorMessage">A mensagem de erro da operação falha.</param>
    /// <returns>Uma instância de <see cref="Result{T}"/>, indicando o fracasso da operação.</returns>
    public new static Result<T> Failure(string errorKey, string errorMessage)
    {
        ResultDictionary errors = [];

        errors.AddError(errorKey, errorMessage);

        return new Result<T>(succeeded: false, errors, data: default);
    }
}
