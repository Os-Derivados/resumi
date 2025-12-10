using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;
using Resumi.App.Modules;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/resumes/{resumeId:int}/volunteerships")]
[Authorize]
public class VolunteershipsController : ControllerBase
{
    private readonly VolunteershipsModule _module;

    public VolunteershipsController(VolunteershipsModule module)
    {
        _module = module;
    }

    [HttpPost]
    public IActionResult Create(int resumeId, [FromBody] AddVolunteershipModel model)
    {
        throw new NotImplementedException("Volunteership creation is not implemented yet.");
    }

    [HttpGet]
    public IActionResult ReadAll(int resumeId)
    {
        throw new NotImplementedException("Retrieving volunteerships for a resume is not implemented yet.");
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int resumeId, int id, [FromBody] UpdateVolunteershipModel model)
    {
        throw new NotImplementedException("Updating a volunteership is not implemented yet.");
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int resumeId, int id)
    {
        throw new NotImplementedException("Deleting a volunteership is not implemented yet.");
    }
}