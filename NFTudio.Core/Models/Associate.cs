namespace NFTudio.Core.Models;
public class Associate
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Benefit { get; set; } = string.Empty;
    public string Situation { get; set; } = string.Empty;
    public string AssociateImagemUrl { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public ICollection<AssociateOperation> AssociateOperations { get; set; } = [];
    public ICollection<Link> Links { get; set; } = [];
}