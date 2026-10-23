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
using Swashbuckle.AspNetCore.Annotations;

namespace OpportPortal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[SwaggerTag("Gerenciamento de empresas — cadastro, consulta e manutenção do perfil empresarial")]
public class CompanyController : BaseController
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Cadastra uma nova empresa",
        Description = "Cria o perfil da empresa vinculado ao usuário autenticado. O CNPJ é validado e não pode estar duplicado.")]
    [SwaggerResponse(201, "Empresa criada com sucesso")]
    [SwaggerResponse(400, "Dados inválidos ou CNPJ já cadastrado")]
    [SwaggerResponse(401, "Usuário não autenticado")]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRequest request)
    {
        var userId = GetUserId();

        var result = await _mediator.DispatchAsync<CreateCompanyCommand, ResultViewModel<long>>(request.CreateCompanyRequestToCommand(userId));

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
    }

    [HttpGet("{id:long}")]
    [SwaggerOperation(
        Summary = "Busca uma empresa por ID",
        Description = "Retorna os dados cadastrais de uma empresa específica.")]
    [SwaggerResponse(200, "Empresa encontrada", typeof(ResultViewModel<CompanyResponse>))]
    [SwaggerResponse(404, "Empresa não encontrada")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _mediator.DispatchAsync<GetCompanyByIdQuery, ResultViewModel<CompanyResponse>>(
            new GetCompanyByIdQuery(id));

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Lista todas as empresas",
        Description = "Retorna a lista de empresas cadastradas na plataforma.")]
    [SwaggerResponse(200, "Lista retornada com sucesso", typeof(ResultViewModel<IEnumerable<CompanyResponse>>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.DispatchAsync<GetAllCompaniesQuery, ResultViewModel<IEnumerable<CompanyResponse>>>(
            new GetAllCompaniesQuery());

        return Ok(result);
    }

    [HttpPut("{id:long}")]
    [Authorize(Roles = "Company")]
    [SwaggerOperation(
        Summary = "Atualiza os dados da empresa",
        Description = "Atualiza o perfil da empresa (nome, setor, telefone e endereço). CNPJ e e-mail não são alterados após o cadastro.")]
    [SwaggerResponse(200, "Empresa atualizada com sucesso")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(403, "Usuário sem permissão")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCompanyRequest request)
    {
        var result = await _mediator.DispatchAsync<UpdateCompanyCommand, ResultViewModel<long>>(request.UpdateCompanyRequestToCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Company")]
    [SwaggerOperation(
        Summary = "Exclui a empresa",
        Description = "Realiza a exclusão do cadastro da empresa.")]
    [SwaggerResponse(200, "Empresa excluída com sucesso")]
    [SwaggerResponse(403, "Usuário sem permissão")]
    [SwaggerResponse(404, "Empresa não encontrada")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _mediator.DispatchAsync<DeleteCompanyCommand, ResultViewModel<long>>(
            new DeleteCompanyCommand(id));

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize(Roles = "Company")]
    [SwaggerOperation(
        Summary = "Retorna a empresa do usuário autenticado",
        Description = "Busca o perfil da empresa vinculada ao token de acesso atual.")]
    [SwaggerResponse(200, "Empresa encontrada", typeof(ResultViewModel<CompanyResponse>))]
    [SwaggerResponse(404, "Nenhuma empresa vinculada ao usuário")]
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
