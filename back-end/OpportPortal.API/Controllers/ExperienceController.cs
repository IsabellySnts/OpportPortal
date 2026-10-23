using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpportPortal.API.Mappers.Experiences;
using OpportPortal.API.Requests;
using OpportPortal.Application.Commands.Experiences.CreateExperience;
using OpportPortal.Application.Commands.Experiences.DeleteExperience;
using OpportPortal.Application.Commands.Experiences.UpdateExperience;
using OpportPortal.Application.Common;
using OpportPortal.Application.Queries.Experiences;
using OpportPortal.Application.Queries.Experiences.GetExperienceById;
using OpportPortal.Application.Queries.Experiences.GetExperiencesByCurriculumId;
using Swashbuckle.AspNetCore.Annotations;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[SwaggerTag("Gerenciamento de experiências profissionais — histórico de cargos do currículo do candidato")]

public class ExperienceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExperienceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Adiciona uma experiência ao currículo",
        Description = "Cadastra uma experiência profissional (cargo, empresa, período e descrição) vinculada a um currículo.")]
    [SwaggerResponse(201, "Experiência criada com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou currículo não encontrado")]
    [SwaggerResponse(403, "Usuário sem permissão (não é um candidato)")]

    public async Task<IActionResult> Create([FromBody] CreateExperienceRequest request)
    {
        var result = await _mediator.DispatchAsync<CreateExperienceCommand, ResultViewModel<long>>(request.CreateExperienceRequestToCommand());

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:long}")]
    [Authorize(Roles = "Candidate,Company,Adm")]
    [SwaggerOperation(
        Summary = "Busca uma experiência por ID",
        Description = "Retorna os dados de uma experiência profissional específica.")]
    [SwaggerResponse(200, "Experiência encontrada", typeof(ResultViewModel<ExperienceResponse>))]
    [SwaggerResponse(404, "Experiência não encontrada")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetExperienceByIdQuery, ResultViewModel<ExperienceResponse>>(
            new GetExperienceByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("curriculum/{curriculumId:long}")]
    [SwaggerOperation(
        Summary = "Lista as experiências de um currículo",
        Description = "Retorna todas as experiências profissionais vinculadas a um currículo. Usado pela empresa ao avaliar um inscrito.")]
    [SwaggerResponse(200, "Experiências retornadas com sucesso", typeof(ResultViewModel<IEnumerable<ExperienceResponse>>))]
    public async Task<IActionResult> GetByCurriculumId(long curriculumId)
    {
        var result = await _mediator.DispatchAsync<GetExperiencesByCurriculumIdQuery, ResultViewModel<IEnumerable<ExperienceResponse>>>(
            new GetExperiencesByCurriculumIdQuery(curriculumId));

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Atualiza uma experiência",
        Description = "Atualiza os dados de uma experiência profissional do currículo.")]
    [SwaggerResponse(200, "Experiência atualizada com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou Id divergente")]
    [SwaggerResponse(403, "Usuário sem permissão")]
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
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Exclui uma experiência",
        Description = "Realiza a exclusão lógica (soft delete) de uma experiência profissional do currículo.")]
    [SwaggerResponse(200, "Experiência excluída com sucesso")]
    [SwaggerResponse(403, "Usuário sem permissão")]
    [SwaggerResponse(404, "Experiência não encontrada")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteExperienceCommand, ResultViewModel<long>>(
            new DeleteExperienceCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}