namespace NFTudio.Core.Requests.Associate;

public class GetAllAssociateManageRequest : PagedRequest
{
    public string? Search { get; set; }
    public ICollection<string>? OperationNames { get; set; }
    public string? Situation { get; set; } = string.Empty;
    public string? Location { get; set; }
}