using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;
using Resumi.App.Data.Models;
using Resumi.App.Modules;
using Resumi.Infra.Auth;
using Resumi.Infra.Data.Interfaces;
using Resumi.Infra.Data.Models;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/resumes")]
[Authorize]
public class ResumesController : ControllerBase

{
    private readonly ResumesModule _module;
    private readonly IResumeMapper _mapper;
    private readonly UserContext _userContext;

    public ResumesController(ResumesModule module, IResumeMapper mapper, UserContext userContext)
    {
        _module = module;
        _mapper = mapper;
        _userContext = userContext;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<ResumeModel>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<ResumeModel>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<ResumeModel>>> Create([Required] string title)
    {
        var userId = _userContext.GetUserId();

        Resume newResume = new() 
        {
            Title = title,
            UserId = userId,
            OwnerName = HttpContext.User.FindFirstValue(ClaimTypes.Name)
        };

        var creationResult = await _module.Service.CreateAsync(newResume);

        if (!creationResult.Succeeded)

        {
            return BadRequest(creationResult);
        }

        var dto = _mapper.ToDto(creationResult.Data);

        if (dto is null) return UnprocessableEntity();

        return Created($"api/resumes/{dto.Id}", Result<ResumeModel>.Success(dto));
    }

    [HttpGet("{id:int}")]
    public IActionResult Read(int id)
    {
        throw new NotImplementedException("Retrieving a resume by ID is not implemented yet.");
    }

    [HttpGet]
    public IActionResult ReadAll([Required] int userId, int skip = 0, int take = 20)
    {
        throw new NotImplementedException("Retrieving all resumes with pagination is not implemented yet.");
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
