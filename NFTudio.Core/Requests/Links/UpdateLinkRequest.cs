namespace NFTudio.Core.Requests.Links;

public class UpdateLinkRequest : Request
{
    public long AssociateId { get; set; }
    public long Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Type { get; set; } = string.Empty;
}