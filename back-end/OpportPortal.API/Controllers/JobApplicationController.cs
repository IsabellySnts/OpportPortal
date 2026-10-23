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
using Swashbuckle.AspNetCore.Annotations;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[SwaggerTag("Gerenciamento de candidaturas — inscrição em vagas e acompanhamento do processo seletivo")]

public class JobApplicationController : BaseController
{
    private readonly IMediator _mediator;

    public JobApplicationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Candidata-se a uma vaga",
        Description = "Registra a candidatura do candidato autenticado a uma oportunidade. Não é permitido candidatar-se duas vezes à mesma vaga nem a uma vaga encerrada.")]
    [SwaggerResponse(201, "Candidatura realizada com sucesso")]
    [SwaggerResponse(400, "Vaga encerrada, candidatura duplicada ou dados inválidos")]
    [SwaggerResponse(403, "Usuário sem permissão (não é um candidato)")]
    public async Task<IActionResult> Create([FromBody] CreateJobApplicationRequest request)
    {
        var userId = GetUserId();

        var result = await _mediator.DispatchAsync<CreateJobApplicationCommand, ResultViewModel<long>>(request.CreateJobApplicationRequestToCommand(userId));

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:long}")]
    [SwaggerOperation(
        Summary = "Busca uma candidatura por ID",
        Description = "Retorna os dados de uma candidatura específica, com candidato, vaga e status.")]
    [SwaggerResponse(200, "Candidatura encontrada", typeof(ResultViewModel<JobApplicationResponse>))]
    [SwaggerResponse(404, "Candidatura não encontrada")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetJobApplicationByIdQuery, ResultViewModel<JobApplicationResponse>>(
            new GetJobApplicationByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("candidate/{candidateId:long}")]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Lista as candidaturas de um candidato",
        Description = "Retorna todas as candidaturas de um candidato, com o status de cada uma. Usado no acompanhamento pelo candidato.")]
    [SwaggerResponse(200, "Candidaturas retornadas com sucesso", typeof(ResultViewModel<IEnumerable<JobApplicationResponse>>))]
    public async Task<IActionResult> GetByCandidateId(long candidateId)
    {
        var result = await _mediator.DispatchAsync<GetJobApplicationsByCandidateIdQuery, ResultViewModel<IEnumerable<JobApplicationResponse>>>(
            new GetJobApplicationsByCandidateIdQuery(candidateId));

        return Ok(result);
    }

    [HttpGet("opportunity/{opportunityId:long}")]
    [Authorize(Roles = "Company")]
    [SwaggerOperation(
        Summary = "Lista os candidatos de uma vaga",
        Description = "Retorna todas as candidaturas de uma oportunidade. Usado pela empresa para avaliar os inscritos.")]
    [SwaggerResponse(200, "Candidatos retornados com sucesso", typeof(ResultViewModel<IEnumerable<JobApplicationResponse>>))]
    [SwaggerResponse(403, "Usuário sem permissão (não é uma empresa)")]
    public async Task<IActionResult> GetByOpportunityId(long opportunityId)
    {
        var result = await _mediator.DispatchAsync<GetJobApplicationsByOpportunityIdQuery, ResultViewModel<IEnumerable<JobApplicationResponse>>>(
            new GetJobApplicationsByOpportunityIdQuery(opportunityId));

        return Ok(result);
    }

    [HttpPatch("{id:long}/status")]
    [Authorize(Roles = "Company")]
    [SwaggerOperation(
        Summary = "Atualiza o status de uma candidatura",
        Description = "Avança a candidatura no processo seletivo (em análise, entrevista, aprovado, etc.). A transição de status segue um fluxo validado. Ação exclusiva da empresa.")]
    [SwaggerResponse(200, "Status atualizado com sucesso")]
    [SwaggerResponse(400, "Transição de status inválida ou Id divergente")]
    [SwaggerResponse(403, "Usuário sem permissão (não é uma empresa)")]
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
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Cancela uma candidatura",
        Description = "Realiza a exclusão lógica (soft delete) da candidatura. O candidato pode cancelar a própria inscrição.")]
    [SwaggerResponse(200, "Candidatura cancelada com sucesso")]
    [SwaggerResponse(400, "Candidatura já finalizada não pode ser cancelada")]
    [SwaggerResponse(403, "Usuário sem permissão")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteJobApplicationCommand, ResultViewModel<long>>(
            new DeleteJobApplicationCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}