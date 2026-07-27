using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.API.Mappers.JobApplications;
using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.JobApplications.CreateJobApplication;
using OpportPortal.Application.Commands.JobApplications.DeleteJobApplication;
using OpportPortal.Application.Commands.JobApplications.UpdateJobApplicationStatus;
using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.JobApplications;
using OpportPortal.Application.Queries.JobApplications.GetJobApplicationById;
using OpportPortal.Application.Queries.JobApplications.GetJobApplicationsByCandidateId;
using OpportPortal.Application.Queries.JobApplications.GetJobApplicationsByOpportunityId;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class JobApplicationController : BaseController
{
    private readonly IMediator _mediator;

    public JobApplicationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobApplicationRequest request)
    {
        var userId = GetUserId();

        var result = await _mediator.DispatchAsync<CreateJobApplicationCommand, ResultViewModel<long>>(request.CreateJobApplicationRequestToCommand(userId));

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetJobApplicationByIdQuery, ResultViewModel<JobApplicationResponse>>(
            new GetJobApplicationByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("candidate/{candidateId:long}")]
    public async Task<IActionResult> GetByCandidateId(long candidateId)
    {
        var result = await _mediator.DispatchAsync<GetJobApplicationsByCandidateIdQuery, ResultViewModel<IEnumerable<JobApplicationResponse>>>(
            new GetJobApplicationsByCandidateIdQuery(candidateId));

        return Ok(result);
    }

    [HttpGet("opportunity/{opportunityId:long}")]
    public async Task<IActionResult> GetByOpportunityId(long opportunityId)
    {
        var result = await _mediator.DispatchAsync<GetJobApplicationsByOpportunityIdQuery, ResultViewModel<IEnumerable<JobApplicationResponse>>>(
            new GetJobApplicationsByOpportunityIdQuery(opportunityId));

        return Ok(result);
    }

    [HttpPatch("{id:long}/status")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateJobApplicationStatusCommand request)
    {
        if (id != request.Id)
            return BadRequest("Id mismatch.");

        var result = await _mediator.DispatchAsync<UpdateJobApplicationStatusCommand, ResultViewModel<long>>(request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteJobApplicationCommand, ResultViewModel<long>>(
            new DeleteJobApplicationCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}