using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;
using Resumi.App.Modules;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UsersModule _module;

    public UsersController(UsersModule module)
    {
        _module = module;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateUserModel model)
    {
        var newUser = _module.Mapper.NewDomainModel(model);
        var userService = (IUserService)_module.Service;
        var creationResult = await userService.CreateAsync(newUser, model.Password);

        if (!creationResult.Succeeded)
        {
            return BadRequest(creationResult);
        }

        var createdUser = _module.Mapper.ToDto(creationResult.Data);

        if (createdUser is null) return UnprocessableEntity();

        return Created($"/api/users/{createdUser.Id}", Result<UserModel>.Success(createdUser));
    }

    [HttpGet("/me")]
    public IActionResult GetAuthor()
    {
        throw new NotImplementedException("Retrieving the current user is not implemented yet.");
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Read(int id)
    {
        var userService = (IUserService)_module.Service;
        var findResult  = await userService.FindAsync(id);

        if (!findResult.Succeeded)
        {
            return BadRequest(findResult);
        }

        var findUser = _module.Mapper.ToDto(findResult.Data);

        if (findUser is null) return UnprocessableEntity();

        return Created($"/api/users/{findUser.Id}", Result<UserModel>.Success(findUser));
    }

    [HttpGet]
    public IActionResult ReadAll(int skip = 0, int take = 20)
    {
        throw new NotImplementedException("Retrieving all users is not implemented yet.");
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] UpdateUserModel model)
    {
        throw new NotImplementedException("Updating a user is not implemented yet.");
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        throw new NotImplementedException("Deleting a user is not implemented yet.");
    }
}
