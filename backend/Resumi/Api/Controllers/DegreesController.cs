using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resumi.Api.Data.Models;
using Resumi.App.Modules;
using Resumi.Infra.Data.Models;

namespace Resumi.Api.Controllers;

[ApiController]
[Route("api/resumes/{resumeId:int}/degrees")]
[AllowAnonymous]
public class DegreesController : ControllerBase
{
    private readonly DegreesModule _module;

    public DegreesController(DegreesModule module)
    {
        _module = module;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<DegreeModel>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<DegreeModel>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<DegreeModel>>> Create(int resumeId, [FromBody] AddDegreeModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Result<DegreeModel> 
            { 
                Succeeded = false, 
                Errors = new() { { "validation", new List<string> { "Modelo de dados inválido" } } }
            });
        }

        if (model.ResumeId != resumeId)
        {
            return BadRequest(new Result<DegreeModel>
            {
                Succeeded = false,
                Errors = new() { { "ResumeId", new List<string> { "ResumeId do corpo não corresponde ao da rota" } } }
            });
        }

        var newDegree = new Degree
        {
            ResumeId = model.ResumeId,
            Name = model.Name,
            Description = model.Description,
            InstitutionName = model.InstitutionName,
            Location = model.Location,
            IsRemote = model.IsRemote,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            StillEngaged = model.StillEngaged,
            Highlights = model.Highlights,
            Level = Enum.Parse<DegreeLevel>(model.DegreeLevel ?? "Unknown")
        };

        var creationResult = await _module.Service.CreateAsync(newDegree);

        if (!creationResult.Succeeded)
        {
            return BadRequest(creationResult);
        }

        var dto = MapToDto(creationResult.Data);

        return Created($"api/resumes/{resumeId}/degrees/{dto.Id}", Result<DegreeModel>.Success(dto));
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<List<DegreeModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<List<DegreeModel>>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<List<DegreeModel>>>> ReadAll(int resumeId, int skip = 0, int take = 20)
    {
        var result = await _module.Service.FindAllAsync(skip, take);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        var degrees = result.Data
            ?.Where(d => d.ResumeId == resumeId)
            .Select(MapToDto)
            .ToList() ?? new List<DegreeModel>();

        return Ok(Result<List<DegreeModel>>.Success(degrees));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Result<DegreeModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<DegreeModel>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result<DegreeModel>>> Update(int resumeId, int id, [FromBody] UpdateDegreeModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Result<DegreeModel>
            {
                Succeeded = false,
                Errors = new() { { "validation", new List<string> { "Modelo de dados inválido" } } }
            });
        }

        if (model.Id != id)
        {
            return BadRequest(new Result<DegreeModel>
            {
                Succeeded = false,
                Errors = new() { { "Id", new List<string> { "ID do corpo não corresponde ao da rota" } } }
            });
        }

        var findResult = await _module.Service.FindAsync(id);
        if (!findResult.Succeeded)
        {
            return BadRequest(findResult);
        }

        var currentDegree = findResult.Data;
        if (currentDegree?.ResumeId != resumeId)
        {
            return BadRequest(new Result<DegreeModel>
            {
                Succeeded = false,
                Errors = new() { { "ResumeId", new List<string> { "Formação não pertence a este currículo" } } }
            });
        }

        var updatedDegree = new Degree
        {
            Id = id,
            ResumeId = resumeId,
            Name = model.Name ?? currentDegree.Name,
            Description = model.Description ?? currentDegree.Description,
            InstitutionName = model.InstitutionName ?? currentDegree.InstitutionName,
            Location = model.Location ?? currentDegree.Location,
            IsRemote = model.IsRemote ?? currentDegree.IsRemote,
            StartDate = model.StartDate ?? currentDegree.StartDate,
            EndDate = model.EndDate ?? currentDegree.EndDate,
            StillEngaged = model.StillEngaged ?? currentDegree.StillEngaged,
            Highlights = model.Highlights ?? currentDegree.Highlights,
            Level = string.IsNullOrEmpty(model.DegreeLevel) 
                ? currentDegree.Level 
                : Enum.Parse<DegreeLevel>(model.DegreeLevel),
            CreatedAt = currentDegree.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };

        var updateResult = await _module.Service.UpdateAsync(currentDegree, updatedDegree);

        if (!updateResult.Succeeded)
        {
            return BadRequest(updateResult);
        }

        var dto = MapToDto(updateResult.Data);

        return Ok(Result<DegreeModel>.Success(dto));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Result<bool>>> Delete(int resumeId, int id)
    {
        var findResult = await _module.Service.FindAsync(id);
        if (!findResult.Succeeded)
        {
            return NotFound(findResult);
        }

        var degree = findResult.Data;
        if (degree?.ResumeId != resumeId)
        {
            return BadRequest(new Result<bool>
            {
                Succeeded = false,
                Errors = new() { { "ResumeId", new List<string> { "Formação não pertence a este currículo" } } }
            });
        }

        var deleteResult = await _module.Service.DeleteAsync(id);

        if (!deleteResult.Succeeded)
        {
            return BadRequest(deleteResult);
        }

        return Ok(deleteResult);
    }

    private static DegreeModel MapToDto(Degree degree)
    {
        return new DegreeModel
        {
            Id = degree.Id,
            Name = degree.Name,
            Description = degree.Description,
            InstitutionName = degree.InstitutionName,
            Location = degree.Location,
            IsRemote = degree.IsRemote,
            StartDate = degree.StartDate,
            EndDate = degree.EndDate,
            StillEngaged = degree.StillEngaged,
            Highlights = degree.Highlights,
            Level = degree.Level.ToString()
        };
    }
}