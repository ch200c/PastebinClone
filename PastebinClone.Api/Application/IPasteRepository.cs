using PastebinClone.Api.Domain.Entities;
using System.Linq.Expressions;

namespace PastebinClone.Api.Application;

public interface IPasteRepository
{
    IUnitOfWork UnitOfWork { get; }

    Task<Paste?> GetPasteAsync(Expression<Func<Paste, bool>> predicate, CancellationToken cancellationToken);
    Paste CreatePaste(Paste paste);
}
