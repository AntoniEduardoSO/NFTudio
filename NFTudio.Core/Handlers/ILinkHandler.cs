using NFTudio.Core.Models;
using NFTudio.Core.Requests.Links;
using NFTudio.Core.Responses;

namespace NFTudio.Core.Handlers;

public interface ILinkHandler
{
    public Task<Response<ICollection<LinkDto?>>> CreateAsync(CreateLinkRequest request);
    public Task<Response<LinkDto?>> UpdateAsync(UpdateLinkRequest request);
     public Task<Response<LinkDto?>> DeleteAsync(DeleteLinkRequest request);
}
