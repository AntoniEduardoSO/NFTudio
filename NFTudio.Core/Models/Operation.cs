namespace NFTudio.Core.Models;
public class Operation
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<AssociateOperation> AssociateOperations { get; set; } = [];
}