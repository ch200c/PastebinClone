using Microsoft.AspNetCore.Mvc;
using PastebinClone.Api.Application;
using PastebinClone.Api.Application.Models;
using PastebinClone.Api.Domain.Entities;

namespace PastebinClone.Api.Controllers;

[ApiController]
[Route("v1/pastes")]
public class PasteController : ControllerBase
{
    private readonly IPasteRepository _pasteRepository;
    private readonly IAliasRepository _aliasRepository;

    public PasteController(IPasteRepository pasteRepository, IAliasRepository aliasRepository)
    {
        _pasteRepository = pasteRepository;
        _aliasRepository = aliasRepository;
    }

    [HttpGet("{id}", Name = "Get paste")]
    public async Task<ActionResult<PasteResponse>> Get(
        [FromRoute] long id, CancellationToken cancellationToken)
    {
        var entity = await _pasteRepository.GetPasteAsync(paste => paste.Id == id, cancellationToken);

        if (entity == null)
        {
            return NotFound();
        }

        var response = MapPasteResponse(entity);
        return Ok(response);
    }

    [HttpPost("", Name = "Create paste")]
    public async Task<ActionResult> Create(
        [FromBody] CreatePasteRequest request, CancellationToken cancellationToken)
    {
        Paste? paste = null;

        if (request.Alias == null)
        {
            var alias = (await _aliasRepository.GetAliasesAsync(isAvailable: true, limit: 1, cancellationToken))
                .Single();

            alias.IsAvailable = false;
            _aliasRepository.UpdateAlias(alias);

            await _aliasRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            paste = new Paste()
            {
                AliasId = alias.Id,
                Alias = alias,
                Content = request.Content,
                ExpiresOn = request.ExpiresOn
            };

            _pasteRepository.CreatePaste(paste);
            await _pasteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var alias = await _aliasRepository.GetAliasAsync(
                alias => alias.Alias == request.Alias, cancellationToken);

            if (alias != null)
            {
                return BadRequest($"Alias {request.Alias} already exists");
            }

            alias = new AliasEntity()
            {
                Alias = request.Alias,
                IsAvailable = false
            };

            _aliasRepository.CreateAlias(alias);
            await _aliasRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            paste = new Paste()
            {
                AliasId = alias.Id,
                Alias = alias,
                Content = request.Content,
                ExpiresOn = request.ExpiresOn
            };

            _pasteRepository.CreatePaste(paste);
            await _pasteRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        var routeValues = new { id = paste.Id };
        var response = MapPasteResponse(paste);

        return CreatedAtAction(nameof(Get), routeValues, response);
    }

    private static PasteResponse MapPasteResponse(Paste paste)
    {
        return new PasteResponse(paste.Id, paste.ExpiresOn, paste.Content, paste.AliasId);
    }
}