namespace PastebinClone.Api.Application.Models;

public record AliasResponse(long Id, string Alias, bool IsAvailable);

public record PasteResponse(long Id, DateTime ExpiresOn, string Content, long AliasId);