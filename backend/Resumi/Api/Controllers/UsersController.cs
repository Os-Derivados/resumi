using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resumi.Api.Data.Models;
using Resumi.Api.Data.Requests;
using Resumi.App.Managers;
using Resumi.Domain.Exceptions;
using Resumi.Domain.Models;
using Resumi.Infra.AuthZ;
using Resumi.Infra.AuthZ.Interfaces;
using Resumi.Infra.Constants;
using Resumi.Infra.Data.Mappers;
using Resumi.Infra.Data.Models;
using Resumi.Infra.Database.Context;
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
	UserManager manager,
	UserManager<AppUser> identityManager,
	AppDbContext dbContext) : ControllerBase
{
	[HttpPost]
	[AllowAnonymous]
	[ProducesResponseType<Result<UserModel>>(StatusCodes.Status201Created)]
	public async Task<IActionResult> Create([FromBody] CreateUserRequest model, CancellationToken cancellation)
	{
		var newUser = mapper.NewDomainModel(model);
		var creationResult = await manager.CreateAsync(newUser, model.Password, cancellation);

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
	public async Task<IActionResult> Read(int id, CancellationToken cancellation)
	{
		var result = await manager.FindAsync(id, cancellation);

		if (!result.Succeeded) return BadRequest(result);

		return Ok(result);
	}

	[HttpGet]
	[Authorize(Roles = AuthConstants.AdminRole)]
	[ProducesResponseType<Result<List<UserModel>>>(StatusCodes.Status200OK)]
	public async Task<IActionResult> ReadAll(CancellationToken cancellation, int skip = 0, int take = 20)
	{
		var result = await manager.FindAllAsync(cancellation, skip, take);

		if (!result.Succeeded) return BadRequest(result);

		return Ok(result);
	}

	[HttpPut("{id:int}")]
	[ProducesResponseType<Result<List<UserModel>>>(StatusCodes.Status200OK)]
	public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest model, CancellationToken cancellation)
	{
		var sessionUser = UserModel.FromClaimsPrincipal(HttpContext.User)
		                  ?? throw new InfrastructureException("An user should be authenticated at this point.");

		if (sessionUser.Id != id && !User.IsInRole(AuthConstants.AdminRole))
		{
			return Forbid();
		}

		var current = await identityManager.FindByEmailAsync(id.ToString());
		var updated = mapper.UpdatedDomainModel(model, current);
		var result = await manager.UpdateAsync(current, updated, cancellation);

		if (!result.Succeeded) return BadRequest(result);

		return Ok(result);
	}

	[HttpDelete("{id:int}")]
	[Authorize(Roles = AuthConstants.AdminRole)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
	{
		try
		{
			var result = await manager.DeleteAsync(id, cancellation);

			if (!result.Succeeded) return BadRequest(result);

			return NoContent();
		}
		catch (Exception ex) when (ex is TimeoutException or OperationCanceledException)
		{
			return UnprocessableEntity();
		}
		catch (DomainException ex)
		{
			return BadRequest(Result.Failure(nameof(AppUser), ex.Message));
		}
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<IActionResult> Login([FromBody] LoginRequest model, CancellationToken cancellation)
	{
		try
		{
			var emailUpper = model.Email.ToUpperInvariant();
			var user = await dbContext.Users
				.AsNoTracking()
				.Include(u => u.UserRoles!)
				.ThenInclude(ur => ur.Role)
				.FirstOrDefaultAsync(u => u.NormalizedEmail == emailUpper, cancellation);

			if (user is null) return NotFound();

			var passwordValid = await identityManager.CheckPasswordAsync(user, model.Password);

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
		catch (Exception ex) when (ex is TimeoutException or OperationCanceledException)
		{
			return UnprocessableEntity();
		}
		catch (DomainException ex)
		{
			return BadRequest(Result.Failure(nameof(AppUser), ex.Message));
		}
	}
}
