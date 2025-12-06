using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public IActionResult Create([FromBody] CreateUserModel model)
    {
        throw new NotImplementedException("User creation is not implemented yet.");
    }

    [HttpGet("/me")]
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
}
