namespace PastebinClone.Api.Domain.Entities;

public class AliasEntity
{
    public long Id { get; set; }
    public required string Alias { get; set; }
    public bool IsAvailable { get; set; }

    public Paste? Paste { get; set; }
}
