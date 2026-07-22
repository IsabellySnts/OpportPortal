using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.Application.Commands.Experiences.CreateExperience;
using OpportPortal.Application.Commands.Experiences.DeleteExperience;
using OpportPortal.Application.Commands.Experiences.UpdateExperience;
using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Experiences;
using OpportPortal.Application.Queries.Experiences.GetExperienceById;
using OpportPortal.Application.Queries.Experiences.GetExperiencesByCurriculumId;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExperienceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExperienceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExperienceCommand request)
    {
        var result = await _mediator.DispatchAsync<CreateExperienceCommand, ResultViewModel<long>>(request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetExperienceByIdQuery, ResultViewModel<ExperienceResponse>>(
            new GetExperienceByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("curriculum/{curriculumId:long}")]
    public async Task<IActionResult> GetByCurriculumId(long curriculumId)
    {
        var result = await _mediator.DispatchAsync<GetExperiencesByCurriculumIdQuery, ResultViewModel<IEnumerable<ExperienceResponse>>>(
            new GetExperiencesByCurriculumIdQuery(curriculumId));

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateExperienceCommand request)
    {
        if (id != request.Id)
            return BadRequest("Id mismatch.");

        var result = await _mediator.DispatchAsync<UpdateExperienceCommand, ResultViewModel<long>>(request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteExperienceCommand, ResultViewModel<long>>(
            new DeleteExperienceCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}