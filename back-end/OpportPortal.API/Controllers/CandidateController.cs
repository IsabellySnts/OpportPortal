using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.API.Mappers.Candidates;
using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.Candidates.CreateCandidate;
using OpportPortal.Application.Commands.Candidates.DeleteCandidate;
using OpportPortal.Application.Commands.Candidates.UpdateCandidate;
using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Candidates;
using OpportPortal.Application.Queries.Candidates.GetAllCandidates;
using OpportPortal.Application.Queries.Candidates.GetCandidateById;
using OpportPortal.Application.Queries.Candidates.GetCandidateByUserId;

namespace OpportPortal.API.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CandidateController : BaseController
{
    private readonly IMediator _mediator;

    public CandidateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCandidateRequest request)
    {
        var userId = GetUserId(); 

        var result = await _mediator.DispatchAsync<CreateCandidateCommand, ResultViewModel<long>>(request.CreateCandidateRequestToCommand(userId));

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(Create), new { id = result.Data }, result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCandidateRequest request)
    {
        var result = await _mediator.DispatchAsync<UpdateCandidateCommand, ResultViewModel<long>>(request.UpdateCandidateRequestToCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteCandidateCommand, ResultViewModel<long>>(
            new DeleteCandidateCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetCandidateByIdQuery, ResultViewModel<CandidateResponse>>(
            new GetCandidateByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.DispatchAsync<GetAllCandidatesQuery, ResultViewModel<IEnumerable<CandidateResponse>>>(
            new GetAllCandidatesQuery());

        return Ok(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = GetUserId();

        var result = await _mediator.DispatchAsync<GetCandidateByUserIdQuery, ResultViewModel<CandidateResponse>>(
            new GetCandidateByUserIdQuery(userId));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
}
