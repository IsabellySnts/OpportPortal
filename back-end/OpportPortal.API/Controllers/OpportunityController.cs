using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.Application.Commands.Opportunities.CloseOpportunity;
using OpportPortal.Application.Commands.Opportunities.CreateOpportunity;
using OpportPortal.Application.Commands.Opportunities.DeleteOpportunity;
using OpportPortal.Application.Commands.Opportunities.UpdateOpportunity;
using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Opportunities;
using OpportPortal.Application.Queries.Opportunities.GetAllOpportunities;
using OpportPortal.Application.Queries.Opportunities.GetOpportunitiesByCompanyId;
using OpportPortal.Application.Queries.Opportunities.GetOpportunitiesByFilter;
using OpportPortal.Application.Queries.Opportunities.GetOpportunityById;
using OpportPortal.Domain.Enums;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OpportunityController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpportunityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOpportunityCommand request)
    {
        var result = await _mediator.DispatchAsync<CreateOpportunityCommand, ResultViewModel<long>>(request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetOpportunityByIdQuery, ResultViewModel<OpportunityResponse>>(
            new GetOpportunityByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.DispatchAsync<GetAllOpportunitiesQuery, ResultViewModel<IEnumerable<OpportunityResponse>>>(
            new GetAllOpportunitiesQuery());

        return Ok(result);
    }

    [HttpGet("filter")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByFilter(
        [FromQuery] string? fieldOfWork,
        [FromQuery] string? state,
        [FromQuery] OpportunityType? type,
        [FromQuery] JobFormat? format)
    {
        var result = await _mediator.DispatchAsync<GetOpportunitiesByFilterQuery, ResultViewModel<IEnumerable<OpportunityResponse>>>(
            new GetOpportunitiesByFilterQuery(fieldOfWork, state, type, format));

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateOpportunityCommand request)
    {
        if (id != request.Id)
            return BadRequest("Id mismatch.");

        var result = await _mediator.DispatchAsync<UpdateOpportunityCommand, ResultViewModel<long>>(request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("{id:long}/close")]
    public async Task<IActionResult> Close(long id)
    {
        var result = await _mediator.DispatchAsync<CloseOpportunityCommand, ResultViewModel<long>>(
            new CloseOpportunityCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteOpportunityCommand, ResultViewModel<long>>(
            new DeleteOpportunityCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("company/{companyId:long}")]
    public async Task<IActionResult> GetByCompany(long companyId)
    {
        var result = await _mediator.DispatchAsync<GetOpportunitiesByCompanyIdQuery, ResultViewModel<IEnumerable<OpportunityResponse>>>(
            new GetOpportunitiesByCompanyIdQuery(companyId));

        return Ok(result);
    }
}