using Microsoft.EntityFrameworkCore;
using PastebinClone.Api.Application;
using PastebinClone.Api.Domain.Entities;
using System.Linq.Expressions;

namespace PastebinClone.Api.Infrastructure.Persistence;

public class AliasRepository : IAliasRepository
{
    private readonly ApplicationDbContext _dbContext;

    public IUnitOfWork UnitOfWork => _dbContext;

    public AliasRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<AliasEntity>> GetAliasesAsync(
        bool? isAvailable, int? limit, CancellationToken cancellationToken)
    {
        IQueryable<AliasEntity> query = _dbContext
            .Aliases
            .OrderBy(alias => alias.Id);

        if (isAvailable != null)
        {
            query = query.Where(alias => alias.IsAvailable == isAvailable);
        }

        limit ??= 10;

        return await query
            .Take(limit.Value)
            .ToListAsync(cancellationToken);
    }

    public async Task<AliasEntity?> GetAliasAsync(
        Expression<Func<AliasEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Aliases
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public void UpdateAlias(AliasEntity alias)
    {
        _dbContext.Entry(alias).State = EntityState.Modified;
    }

    public AliasEntity CreateAlias(AliasEntity alias)
    {
        _dbContext.Aliases.Add(alias);
        return alias;
    }
}
