using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.API.Mappers.Companies;
using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.Companies.CreateCompany;
using OpportPortal.Application.Commands.Companies.DeleteCompany;
using OpportPortal.Application.Commands.Companies.UpdateCompany;
using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Companies;
using OpportPortal.Application.Queries.Companies.GetAllCompanies;
using OpportPortal.Application.Queries.Companies.GetCompanyById;
using OpportPortal.Application.Queries.Companies.GetCompanyByUserId;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompanyController : BaseController
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRequest request)
    {
        var userId = GetUserId();

        var result = await _mediator.DispatchAsync<CreateCompanyCommand, ResultViewModel<long>>(request.CreateCompanyRequestToCommand(userId));

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetCompanyByIdQuery, ResultViewModel<CompanyResponse>>(
            new GetCompanyByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.DispatchAsync<GetAllCompaniesQuery, ResultViewModel<IEnumerable<CompanyResponse>>>(
            new GetAllCompaniesQuery());

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCompanyRequest request)
    {
        var result = await _mediator.DispatchAsync<UpdateCompanyCommand, ResultViewModel<long>>(request.UpdateCompanyRequestToCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteCompanyCommand, ResultViewModel<long>>(
            new DeleteCompanyCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = GetUserId();

        var result = await _mediator.DispatchAsync<GetCompanyByUserIdQuery, ResultViewModel<CompanyResponse>>(
            new GetCompanyByUserIdQuery(userId));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
}
