using NFTudio.Core.Models;

namespace NFTudio.Core.Requests.Links;

public class CreateLinkRequest
{
    public ICollection<LinkDto>? Links { get; set; } = [];
    public long AssociateId { get; set; }    
}
