namespace NFTudio.Core.Models;
public class Link
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Associate Associate { get; set; } = null!;
    public long AssociateId { get; set; }
}
