using Resumi.App.Data.Models;
using Resumi.Infra.Data.Models;

namespace Resumi.App.Services.Interfaces;

/// <summary>
/// Este contrato fornece APIs para validação de operações de domínio
/// em entidades <see cref="Resume"/>.
/// </summary>
public interface IResumeValidator : IDomainValidator<Resume>;