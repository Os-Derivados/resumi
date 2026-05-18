namespace Resumi.Domain.Models;

/// <summary>
/// Este contrato define APIs para gerar cópias de entidades de domínio.
/// </summary>
/// <typeparam name="T">O tipo da entidad de domínio.</typeparam>
public interface ICloneableEntity<T>
{
	/// <summary>
	/// Gera uma cópia rasa (shallow copy) da entidade da instância.
	/// </summary>
	T? ShallowCopy();
}