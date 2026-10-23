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
using Swashbuckle.AspNetCore.Annotations;

namespace OpportPortal.API.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
[SwaggerTag("Gerenciamento de candidatos — cadastro, consulta e manutenção do perfil do estudante")]

public class CandidateController : BaseController
{
    private readonly IMediator _mediator;

    public CandidateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Cadastra um novo candidato",
        Description = "Cria o perfil do candidato vinculado ao usuário autenticado. Exige token de um usuário do tipo Candidato.")]
    [SwaggerResponse(201, "Candidato criado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou candidato já cadastrado para este usuário")]
    [SwaggerResponse(401, "Usuário não autenticado")]
    [SwaggerResponse(403, "Usuário sem permissão (não é um candidato)")]
    public async Task<IActionResult> Create([FromBody] CreateCandidateRequest request)
    {
        var userId = GetUserId(); 

        var result = await _mediator.DispatchAsync<CreateCandidateCommand, ResultViewModel<long>>(request.CreateCandidateRequestToCommand(userId));

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(Create), new { id = result.Data }, result);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Atualiza os dados do candidato",
        Description = "Atualiza o perfil do candidato (nome, data de nascimento, telefone, endereço e LinkedIn).")]
    [SwaggerResponse(200, "Candidato atualizado com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(403, "Usuário sem permissão")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCandidateRequest request)
    {
        var result = await _mediator.DispatchAsync<UpdateCandidateCommand, ResultViewModel<long>>(request.UpdateCandidateRequestToCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Exclui o candidato",
        Description = "Realiza a exclusão lógica (soft delete) do cadastro do candidato.")]
    [SwaggerResponse(200, "Candidato excluído com sucesso")]
    [SwaggerResponse(403, "Usuário sem permissão")]
    [SwaggerResponse(404, "Candidato não encontrado")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteCandidateCommand, ResultViewModel<long>>(
            new DeleteCandidateCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [Authorize(Roles = "Company, Adm")]
    [SwaggerOperation(
        Summary = "Busca um candidato por ID",
        Description = "Retorna os dados cadastrais de um candidato específico.")]
    [SwaggerResponse(200, "Candidato encontrado", typeof(ResultViewModel<CandidateResponse>))]
    [SwaggerResponse(404, "Candidato não encontrado")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetCandidateByIdQuery, ResultViewModel<CandidateResponse>>(
            new GetCandidateByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admn")]
    [SwaggerOperation(
        Summary = "Lista todos os candidatos",
        Description = "Retorna a lista de candidatos cadastrados na plataforma.")]
    [SwaggerResponse(200, "Lista retornada com sucesso", typeof(ResultViewModel<IEnumerable<CandidateResponse>>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.DispatchAsync<GetAllCandidatesQuery, ResultViewModel<IEnumerable<CandidateResponse>>>(
            new GetAllCandidatesQuery());

        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize(Roles = "Candidate")]
    [SwaggerOperation(
        Summary = "Retorna o candidato do usuário autenticado",
        Description = "Busca o perfil do candidato vinculado ao token de acesso atual.")]
    [SwaggerResponse(200, "Candidato encontrado", typeof(ResultViewModel<CandidateResponse>))]
    [SwaggerResponse(404, "Nenhum candidato vinculado ao usuário")]
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
