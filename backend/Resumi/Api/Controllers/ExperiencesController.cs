using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;
using Resumi.App.Modules;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/resumes/{resumeId:int}/experiences")]
[Authorize]
public class ExperiencesController : ControllerBase
{
    private readonly ExperiencesModule _module;

    public ExperiencesController(ExperiencesModule module)
    {
        _module = module;
    }

    [HttpPost]
    public IActionResult Create(int resumeId, [FromBody] AddExperienceModel model)
    {
        throw new NotImplementedException("Experience creation is not implemented yet.");
    }

    [HttpGet]
    public IActionResult ReadAll(int resumeId)
    {
        throw new NotImplementedException("Retrieving experiences for a resume is not implemented yet.");
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int resumeId, int id, [FromBody] UpdateExperienceModel model)
    {
        throw new NotImplementedException("Updating an experience is not implemented yet.");
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int resumeId, int id)
    {
        throw new NotImplementedException("Deleting an experience is not implemented yet.");
    }
}