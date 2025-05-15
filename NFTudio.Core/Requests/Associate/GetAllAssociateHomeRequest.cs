namespace NFTudio.Core.Requests.Associate;
public class GetAllAssociateHomeRequest : PagedRequest
{
    public string? Search {get;set;}
    public ICollection<string>? OperationNames {get;set;}
}