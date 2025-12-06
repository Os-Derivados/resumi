using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/resumes/{resumeId:int}/degrees")]
[Authorize]
public class DegreesController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(int resumeId, [FromBody] AddAcademicDegreeModel model)
    {
        throw new NotImplementedException("Degree creation is not implemented yet.");
    }

    [HttpGet]
    public IActionResult ReadAll(int resumeId)
    {
        throw new NotImplementedException("Retrieving experiences for a resume is not implemented yet.");
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int resumeId, int id, [FromBody] UpdateAcademicDegreeModel model)
    {
        throw new NotImplementedException("Updating a degree is not implemented yet.");
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int resumeId, int id)
    {
        throw new NotImplementedException("Deleting a degree is not implemented yet.");
    }
}