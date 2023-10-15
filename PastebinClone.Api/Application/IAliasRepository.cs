using PastebinClone.Api.Domain.Entities;
using System.Linq.Expressions;

namespace PastebinClone.Api.Application;

public interface IAliasRepository
{
    IUnitOfWork UnitOfWork { get; }

    Task<IEnumerable<AliasEntity>> GetAliasesAsync(bool? isAvailable, int? limit, CancellationToken cancellationToken);
    Task<AliasEntity?> GetAliasAsync(Expression<Func<AliasEntity, bool>> predicate, CancellationToken cancellationToken);
    void UpdateAlias(AliasEntity alias);
    AliasEntity CreateAlias(AliasEntity alias);
}
