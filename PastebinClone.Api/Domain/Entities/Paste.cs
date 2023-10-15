namespace PastebinClone.Api.Domain.Entities;

public class Paste
{
    public long Id { get; set; }
    public DateTime ExpiresOn { get; set; }
    public required string Content { get; set; }
    public required long AliasId { get; set; }

    public required AliasEntity Alias { get; set; }
}
