using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/resumes")]
[Authorize]
public class ResumesController : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] CreateResumeModel model)
    {
        throw new NotImplementedException("Resume creation is not implemented yet.");
    }

    [HttpGet("{id:int}")]
    public IActionResult Read(int id)
    {
        throw new NotImplementedException("Retrieving a resume by ID is not implemented yet.");
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] UpdateResumeModel model)
    {
        throw new NotImplementedException("Updating a resume is not implemented yet.");
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        throw new NotImplementedException("Deleting a resume is not implemented yet.");
    }
}
