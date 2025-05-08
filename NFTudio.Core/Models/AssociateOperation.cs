namespace NFTudio.Core.Models;
public class AssociateOperation
{
    public Associate Associate { get; set; } = null!;
    public long AssociateId { get; set; }
    public Operation Operation { get; set; } = null!;
    public long OperationId { get; set; }
}