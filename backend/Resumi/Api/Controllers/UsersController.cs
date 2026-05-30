using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;
using Resumi.App.Services;
using Resumi.Domain.Models;
using Resumi.Infra.AuthZ;
using Resumi.Infra.Auth.Interfaces;
using Resumi.Infra.Constants;
using Resumi.Infra.Data.Mappers;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Exceptions;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType<Result<UserModel>>(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
public class UsersController(
	IAuthManager authManager,
	UserMapper mapper,
	UserService service,
	UserManager<AppUser> userManager) : ControllerBase
{
	[HttpPost]
	[AllowAnonymous]
	[ProducesResponseType<Result<UserModel>>(StatusCodes.Status201Created)]
	public async Task<IActionResult> Create([FromBody] CreateUserModel model)
	{
		var newUser = mapper.NewDomainModel(model);
		var creationResult = await service.CreateAsync(newUser, model.Password);

		return !creationResult.Succeeded
			? BadRequest(creationResult)
			: Created($"/api/users/{creationResult.Data.Id}", creationResult);
	}

	[HttpGet("me")]
	[ProducesResponseType<Result<UserModel>>(StatusCodes.Status200OK)]
	public IActionResult GetAuthor()
	{
		var sessionUser = UserModel.FromClaimsPrincipal(HttpContext.User);

		if (sessionUser is null) return UnprocessableEntity();

		return Ok(Result<UserModel>.Success(sessionUser));
	}

	[HttpGet("{id:int}")]
	[Authorize(Roles = AuthConstants.AdminRole)]
	[ProducesResponseType<Result<UserModel>>(StatusCodes.Status200OK)]
	[ProducesResponseType<Result<UserModel>>(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Read(int id)
	{
		var result = await service.FindAsync(id);

		if (!result.Succeeded) return BadRequest(result);

		return Ok(result);
	}

	[HttpGet]
	[Authorize(Roles = AuthConstants.AdminRole)]
	[ProducesResponseType<Result<List<UserModel>>>(StatusCodes.Status200OK)]
	public async Task<IActionResult> ReadAll(int skip = 0, int take = 20)
	{
		var result = await service.FindAllAsync(skip, take);

		if (!result.Succeeded) return BadRequest(result);

		return Ok(result);
	}

	[HttpPut("{id:int}")]
	[ProducesResponseType<Result<List<UserModel>>>(StatusCodes.Status200OK)]
	public async Task<IActionResult> Update(int id, [FromBody] UpdateUserModel model)
	{
		var sessionUser = UserModel.FromClaimsPrincipal(HttpContext.User)
		                  ?? throw new InfrastructureException("An user should be authenticated at this point.");

		if (sessionUser.Id != id && !User.IsInRole(AuthConstants.AdminRole))
		{
			return Forbid();
		}

		var current = await userManager.FindByEmailAsync(id.ToString());
		var updated = mapper.UpdatedDomainModel(model, current);
		var result = await service.UpdateAsync(current, updated);

		if (!result.Succeeded) return BadRequest(result);

		return Ok(result);
	}

	[HttpDelete("{id:int}")]
	[Authorize(Roles = AuthConstants.AdminRole)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await service.DeleteAsync(id);

		if (!result.Succeeded) return BadRequest(result);

		return NoContent();
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login([FromBody] LoginModel model)
	{
		var user = await userManager.FindByEmailAsync(model.Email);

		if (user is null) return NotFound();

		var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);

		if (!passwordValid) return Unauthorized();

		var authResponse = authManager.NewAuthResponse(user);

		HttpContext.Response.Cookies.Append(AuthConstants.JwtCookie, authResponse.Token!, new CookieOptions
		{
			HttpOnly = true,
			Secure = true,
			SameSite = SameSiteMode.None,
			Expires = authResponse.ExpiresAt
		});

		return Ok(Result<AuthResponse>.Success(authResponse with { Token = null }));
	}
}
