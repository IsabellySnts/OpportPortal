using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.API.Mappers.Curriculums;
using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.Curriculums.CreateCurriculum;
using OpportPortal.Application.Commands.Curriculums.DeleteCurriculum;
using OpportPortal.Application.Commands.Curriculums.UpdateCurriculum;
using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Curriculums;
using OpportPortal.Application.Queries.Curriculums.GetCurriculumByCandidateId;
using OpportPortal.Application.Queries.Curriculums.GetCurriculumById;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CurriculumController : BaseController
{
    private readonly IMediator _mediator;

    public CurriculumController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCurriculumRequest request)
    {
        var userId = GetUserId();
        var result = await _mediator.DispatchAsync<CreateCurriculumCommand, ResultViewModel<long>>(request.CreateCurriculumRequestToCommand(userId));

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetCurriculumByIdQuery, ResultViewModel<CurriculumResponse>>(
            new GetCurriculumByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("candidate/{candidateId:long}")]
    public async Task<IActionResult> GetByCandidateId(long candidateId)
    {
        var result = await _mediator.DispatchAsync<GetCurriculumByCandidateIdQuery, ResultViewModel<CurriculumResponse>>(
            new GetCurriculumByCandidateIdQuery(candidateId));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCurriculumRequest request)
    {
        var result = await _mediator.DispatchAsync<UpdateCurriculumCommand, ResultViewModel<long>>(request.UpdateCurriculumRequestToCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteCurriculumCommand, ResultViewModel<long>>(
            new DeleteCurriculumCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}