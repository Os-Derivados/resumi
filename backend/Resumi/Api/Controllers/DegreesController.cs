using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;
using Resumi.App.Modules;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/resumes/{resumeId:int}/degrees")]
[Authorize]
public class DegreesController : ControllerBase
{
    private readonly DegreesModule _module;

    public DegreesController(DegreesModule module)
    {
        _module = module;
    }

    [HttpPost]
    public IActionResult Create(int resumeId, [FromBody] AddDegreeModel model)
    {
        throw new NotImplementedException("Degree creation is not implemented yet.");
    }

    [HttpGet]
    public IActionResult ReadAll(int resumeId)
    {
        throw new NotImplementedException("Retrieving degrees for a resume is not implemented yet.");
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int resumeId, int id, [FromBody] UpdateDegreeModel model)
    {
        throw new NotImplementedException("Updating a degree is not implemented yet.");
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int resumeId, int id)
    {
        throw new NotImplementedException("Deleting a degree is not implemented yet.");
    }
}