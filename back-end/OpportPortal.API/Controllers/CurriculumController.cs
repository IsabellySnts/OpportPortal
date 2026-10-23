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
using Swashbuckle.AspNetCore.Annotations;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[SwaggerTag("Gerenciamento de currículos — formação, hard skills e soft skills do candidato")]

public class CurriculumController : BaseController
{
    private readonly IMediator _mediator;

    public CurriculumController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Cadastra o currículo do candidato",
        Description = "Cria o currículo vinculado ao candidato autenticado, com formação, hard skills e soft skills.")]
    [SwaggerResponse(201, "Currículo criado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou currículo já existente para este candidato")]
    [SwaggerResponse(403, "Usuário sem permissão (não é um candidato)")]
    public async Task<IActionResult> Create([FromBody] CreateCurriculumRequest request)
    {
        var userId = GetUserId();
        var result = await _mediator.DispatchAsync<CreateCurriculumCommand, ResultViewModel<long>>(request.CreateCurriculumRequestToCommand(userId));

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:long}")]
    [Authorize(Roles = "Candidate,Company,Adm")]
    [SwaggerOperation(
        Summary = "Busca um currículo por ID",
        Description = "Retorna o currículo de um candidato específico.")]
    [SwaggerResponse(200, "Currículo encontrado", typeof(ResultViewModel<CurriculumResponse>))]
    [SwaggerResponse(404, "Currículo não encontrado")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetCurriculumByIdQuery, ResultViewModel<CurriculumResponse>>(
            new GetCurriculumByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("candidate/{candidateId:long}")]
    [Authorize(Roles = "Candidate,Company,Adm")]
    [SwaggerOperation(
        Summary = "Busca o currículo pelo ID do candidato",
        Description = "Retorna o currículo vinculado a um candidato. Usado pela empresa ao avaliar um inscrito.")]
    [SwaggerResponse(200, "Currículo encontrado", typeof(ResultViewModel<CurriculumResponse>))]
    [SwaggerResponse(404, "Currículo não encontrado")]
    public async Task<IActionResult> GetByCandidateId(long candidateId)
    {
        var result = await _mediator.DispatchAsync<GetCurriculumByCandidateIdQuery, ResultViewModel<CurriculumResponse>>(
            new GetCurriculumByCandidateIdQuery(candidateId));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Atualiza o currículo",
        Description = "Atualiza a formação, as hard skills e as soft skills do currículo do candidato.")]
    [SwaggerResponse(200, "Currículo atualizado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(403, "Usuário sem permissão")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCurriculumRequest request)
    {
        var result = await _mediator.DispatchAsync<UpdateCurriculumCommand, ResultViewModel<long>>(request.UpdateCurriculumRequestToCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Exclui o currículo",
        Description = "Realiza a exclusão lógica (soft delete) do currículo do candidato.")]
    [SwaggerResponse(200, "Currículo excluído com sucesso")]
    [SwaggerResponse(403, "Usuário sem permissão")]
    [SwaggerResponse(404, "Currículo não encontrado")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteCurriculumCommand, ResultViewModel<long>>(
            new DeleteCurriculumCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}