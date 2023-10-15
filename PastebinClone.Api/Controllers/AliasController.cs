using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PastebinClone.Api.Application;
using PastebinClone.Api.Application.Models;
using PastebinClone.Api.Controllers.Examples;
using PastebinClone.Api.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;

namespace PastebinClone.Api.Controllers;

[ApiController, Route("v1/aliases")]
public class AliasController : ControllerBase
{
    private readonly IAliasRepository _aliasRepository;

    public AliasController(IAliasRepository aliasRepository)
    {
        _aliasRepository = aliasRepository;
    }

    [HttpGet("", Name = "Get aliases")]
    public async Task<ActionResult<IEnumerable<AliasResponse>>> Get(
        [FromQuery] bool? isAvailable, [FromQuery] int? limit, CancellationToken cancellationToken)
    {
        var entities = await _aliasRepository.GetAliasesAsync(isAvailable, limit, cancellationToken);
        var response = entities.Select(MapAliasResponse);

        return Ok(response);
    }

    [HttpGet("{id}", Name = "Get alias")]
    public async Task<ActionResult<AliasResponse>> Get(
        [FromRoute] long id, CancellationToken cancellationToken)
    {
        var entity = await _aliasRepository.GetAliasAsync(alias => alias.Id == id, cancellationToken);

        if (entity == null)
        {
            return NotFound();
        }

        var response = MapAliasResponse(entity);
        return Ok(response);
    }

    [HttpPatch("{id}", Name = "Update alias")]
    [SwaggerRequestExample(
        requestType: typeof(object), examplesProviderType: typeof(PatchAliasRequestExampleProvider))]
    public async Task<ActionResult<AliasResponse>> Update(
        [FromRoute] long id,
        [FromBody] JsonPatchDocument<CreateOrUpdateAliasRequest> patch,
        CancellationToken cancellationToken)
    {
        var entityById = await _aliasRepository.GetAliasAsync(alias => alias.Id == id, cancellationToken);

        if (entityById == null)
        {
            return NotFound();
        }

        var request = new CreateOrUpdateAliasRequest(entityById.Alias, entityById.IsAvailable);
        patch.ApplyTo(request, ModelState);

        var entityByAlias = await _aliasRepository.GetAliasAsync(
            alias => alias.Id != id && alias.Alias == request.Alias, cancellationToken);

        if (entityByAlias != null)
        {
            return BadRequest($"Alias {request.Alias} already exists");
        }

        entityById.Alias = request.Alias;
        entityById.IsAvailable = request.IsAvailable;

        _aliasRepository.UpdateAlias(entityById);
        await _aliasRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = MapAliasResponse(entityById);
        return Ok(response);
    }

    [HttpPost("", Name = "Create alias")]
    public async Task<ActionResult<AliasResponse>> Create(
        [FromBody] CreateOrUpdateAliasRequest request, CancellationToken cancellationToken)
    {
        var entityByAlias = await _aliasRepository.GetAliasAsync(
            alias => alias.Alias == request.Alias, cancellationToken);

        if (entityByAlias != null)
        {
            return BadRequest($"Alias {request.Alias} already exists");
        }

        var entity = new AliasEntity()
        {
            Alias = request.Alias,
            IsAvailable = request.IsAvailable
        };

        entity = _aliasRepository.CreateAlias(entity);
        await _aliasRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        var routeValues = new { id = entity.Id };
        var response = MapAliasResponse(entity);

        return CreatedAtAction(nameof(Get), routeValues, response);
    }

    private static AliasResponse MapAliasResponse(AliasEntity alias)
    {
        return new AliasResponse(alias.Id, alias.Alias, alias.IsAvailable);
    }
}