using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;
using Resumi.App.Modules;
using Resumi.App.Services.Interfaces;
using Resumi.Infra.Auth;
using Resumi.Infra.Auth.Interfaces;
using Resumi.Infra.Constants;
using Resumi.Infra.Data.Models;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UsersModule _module;
    private readonly IAuthManager _authManager;

    public UsersController(UsersModule module, IAuthManager authManager)
    {
        _module = module;
        _authManager = authManager;
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

    [HttpGet("me")]
    public IActionResult GetAuthor()
    {
        throw new NotImplementedException("Retrieving the current user is not implemented yet.");
    }

    [HttpGet("{id:int}")]
    public IActionResult Read(int id)
    {
        throw new NotImplementedException("Retrieving a user by ID is not implemented yet.");
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

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _module.UserManager.FindByEmailAsync(model.Email);

        if (user is null) return NotFound();

        var passwordValid = await _module.UserManager.CheckPasswordAsync(user, model.Password);

        if (!passwordValid) return Unauthorized();

        var authResponse = _authManager.NewAuthResponse(user);

        HttpContext.Response.Cookies.Append(AuthConstants.JwtCookie, authResponse.Token!, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = authResponse.ExpiresAt
        });

        return Ok(Result<AuthResponse>.Success(authResponse with { Token = null }));
    }
}
