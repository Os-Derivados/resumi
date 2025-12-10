namespace Resumi.Infra.Data.Models;

/// <summary>
/// Representa um dicionário de resultados que armazena listas de mensagens de erro associadas a chaves específicas.
/// </summary>
public class ResultDictionary : Dictionary<string, List<string>>
{
    public ResultDictionary()
        : base() { }

    public ResultDictionary(IDictionary<string, List<string>> dictionary)
        : base(dictionary) { }

    /// <summary>
    /// Adiciona uma mensagem de erro associada a uma chave específica no dicionário.
    /// </summary>
    /// <param name="key">Uma chave nova ou existente no dicionário.</param>
    /// <param name="errorMessage">A mensagem de erro a ser adicionada.</param>
    public void AddError(string key, string errorMessage)
    {
        if (!ContainsKey(key))
        {
            this[key] = new List<string>();
        }

        this[key].Add(errorMessage);
    }
}
