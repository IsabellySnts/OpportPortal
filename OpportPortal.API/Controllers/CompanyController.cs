using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.Application.Commands.Companies.CreateCompany;
using OpportPortal.Application.Commands.Companies.DeleteCompany;
using OpportPortal.Application.Commands.Companies.UpdateCompany;
using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Companies;
using OpportPortal.Application.Queries.Companies.GetAllCompanies;
using OpportPortal.Application.Queries.Companies.GetCompanyById;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyCommand request)
    {
        var result = await _mediator.DispatchAsync<CreateCompanyCommand, ResultViewModel<long>>(request);

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
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCompanyCommand request)
    {
        if (id != request.Id)
            return BadRequest("Id mismatch.");

        var result = await _mediator.DispatchAsync<UpdateCompanyCommand, ResultViewModel<long>>(request);

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
}
