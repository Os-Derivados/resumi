using Microsoft.AspNetCore.Identity;
using Resumi.App.Data.Models;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Database.Interfaces;

namespace Resumi.App.Modules;

/// <summary>
/// Este componente oferece acesso centralizado a componentes relacionados a um domínio específico.
/// </summary>
/// <typeparam name="TEntity">
/// Uma entidade que implementa a interface ITrackable.
/// </typeparam>
public abstract class DomainModule<TEntity> where TEntity : ITrackable
{
    public readonly IDomainService<TEntity> Service;
    public readonly IDomainValidator<TEntity> Validator;
    public readonly IRepository<TEntity>? Repository;
    public readonly UserManager<AppUser> UserManager;
    public readonly RoleManager<IdentityRole<int>> RoleManager;

    protected DomainModule(
        IDomainService<TEntity> service,
        IDomainValidator<TEntity> validator,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IRepository<TEntity>? repository = null
    )
    {
        Service = service;
        Validator = validator;
        UserManager = userManager;
        RoleManager = roleManager;
        Repository = repository;
    }
}
