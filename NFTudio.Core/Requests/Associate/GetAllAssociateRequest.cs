namespace NFTudio.Core.Requests.Associate;
public class GetAllAssociateRequest : PagedRequest
{
    public string? Search {get;set;}
    public ICollection<string>? OperationNames {get;set;}
}