namespace PastebinClone.Api.Application.Models;

public record CreatePasteRequest(DateTime ExpiresOn, string Content, string? Alias);

public record CreateOrUpdateAliasRequest(string Alias, bool IsAvailable);