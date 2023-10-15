using Microsoft.EntityFrameworkCore;
using PastebinClone.Api.Application;
using PastebinClone.Api.Domain.Entities;
using System.Linq.Expressions;

namespace PastebinClone.Api.Infrastructure.Persistence;

public class PasteRepository : IPasteRepository
{
    private readonly ApplicationDbContext _dbContext;

    public IUnitOfWork UnitOfWork => _dbContext;

    public PasteRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Paste?> GetPasteAsync(
        Expression<Func<Paste, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Pastes
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Paste CreatePaste(Paste paste)
    {
        _dbContext.Add(paste);
        return paste;
    }
}
