namespace Resumi.Domain.Models;

/// <summary>
/// Este contrato define APIs para gerar cópias de entidades de domínio.
/// </summary>
/// <typeparam name="T">O tipo da entidad de domínio.</typeparam>
public interface ICloneableEntity<T> where T : Entity
{
	/// <summary>
	/// Gera uma cópia rasa (shallow copy) da entidade <paramref name="baseEntity"/>.
	/// </summary>
	/// <param name="baseEntity">A entidade original a ser copiada.</param>
	/// <returns>Uma instância de <see cref="T"/>, contendo todos os campos básicos de <paramref name="baseEntity"/>.</returns>
	T? ShallowCopy(T baseEntity);
}