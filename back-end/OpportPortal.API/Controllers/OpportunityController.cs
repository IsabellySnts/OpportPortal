using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.API.Requests;
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
using Swashbuckle.AspNetCore.Annotations;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[SwaggerTag("Gerenciamento de oportunidades — publicação, consulta, filtros e encerramento de vagas")]

public class OpportunityController : BaseController
{
    private readonly IMediator _mediator;

    public OpportunityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Company")]
    [SwaggerOperation(
        Summary = "Publica uma nova oportunidade",
        Description = "Cria uma vaga vinculada à empresa autenticada. Ação exclusiva de empresas, garantindo a confiabilidade das oportunidades.")]
    [SwaggerResponse(201, "Oportunidade publicada com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou empresa não encontrada")]
    [SwaggerResponse(403, "Usuário sem permissão (não é uma empresa)")]
    public async Task<IActionResult> Create([FromBody] CreateOpportunityRequest request)
    {
        var userId = GetUserId();

        var command = new CreateOpportunityCommand(
            userId,
            request.Title,
            request.Description,
            request.Type,
            request.Format,
            request.Requirements,
            request.FieldOfWork,
            request.State,
            request.Salary
        );

        var result = await _mediator.DispatchAsync<CreateOpportunityCommand, ResultViewModel<long>>(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:long}")]
    [SwaggerOperation(
        Summary = "Busca uma oportunidade por ID",
        Description = "Retorna os detalhes de uma vaga específica.")]
    [SwaggerResponse(200, "Oportunidade encontrada", typeof(ResultViewModel<OpportunityResponse>))]
    [SwaggerResponse(404, "Oportunidade não encontrada")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetOpportunityByIdQuery, ResultViewModel<OpportunityResponse>>(
            new GetOpportunityByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Lista todas as oportunidades ativas",
        Description = "Retorna as vagas ativas disponíveis para os candidatos.")]
    [SwaggerResponse(200, "Oportunidades retornadas com sucesso", typeof(ResultViewModel<IEnumerable<OpportunityResponse>>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.DispatchAsync<GetAllOpportunitiesQuery, ResultViewModel<IEnumerable<OpportunityResponse>>>(
            new GetAllOpportunitiesQuery());

        return Ok(result);
    }

    [HttpGet("filter")]
    [SwaggerOperation(
        Summary = "Filtra oportunidades",
        Description = "Busca vagas ativas por área, estado, tipo e formato. Endpoint público — permite consulta sem autenticação.")]
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
    [Authorize(Roles = "Company")]
    [SwaggerOperation(
        Summary = "Atualiza uma oportunidade",
        Description = "Atualiza os dados de uma vaga publicada. Ação exclusiva da empresa.")]
    [SwaggerResponse(200, "Oportunidade atualizada com sucesso")]
    [SwaggerResponse(400, "Dados inválidos, vaga encerrada ou Id divergente")]
    [SwaggerResponse(403, "Usuário sem permissão")]
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
    [Authorize(Roles = "Company")]
    [SwaggerOperation(
        Summary = "Encerra uma oportunidade",
        Description = "Marca a vaga como encerrada, retirando-a da listagem de vagas ativas. Ação exclusiva da empresa.")]
    [SwaggerResponse(200, "Oportunidade encerrada com sucesso")]
    [SwaggerResponse(400, "Vaga já encerrada")]
    [SwaggerResponse(403, "Usuário sem permissão")]
    public async Task<IActionResult> Close(long id)
    {
        var result = await _mediator.DispatchAsync<CloseOpportunityCommand, ResultViewModel<long>>(
            new CloseOpportunityCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Company")]
    [SwaggerOperation(
        Summary = "Exclui uma oportunidade",
        Description = "Realiza a exclusão lógica (soft delete) de uma vaga. Ação exclusiva da empresa.")]
    [SwaggerResponse(200, "Oportunidade excluída com sucesso")]
    [SwaggerResponse(403, "Usuário sem permissão")]
    [SwaggerResponse(404, "Oportunidade não encontrada")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteOpportunityCommand, ResultViewModel<long>>(
            new DeleteOpportunityCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("company/{companyId:long}")]
    [SwaggerOperation(
        Summary = "Lista as vagas de uma empresa",
        Description = "Retorna todas as oportunidades publicadas por uma empresa. Usado no painel da empresa.")]
    [SwaggerResponse(200, "Oportunidades retornadas com sucesso", typeof(ResultViewModel<IEnumerable<OpportunityResponse>>))]
    [SwaggerResponse(403, "Usuário sem permissão")]
    public async Task<IActionResult> GetByCompany(long companyId)
    {
        var result = await _mediator.DispatchAsync<GetOpportunitiesByCompanyIdQuery, ResultViewModel<IEnumerable<OpportunityResponse>>>(
            new GetOpportunitiesByCompanyIdQuery(companyId));

        return Ok(result);
    }
}