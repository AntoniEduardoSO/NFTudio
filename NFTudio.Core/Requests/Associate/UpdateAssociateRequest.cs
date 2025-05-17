using NFTudio.Core.Models;

namespace NFTudio.Core.Requests.Associate;
public class UpdateAssociateRequest : Request
{
    public long Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Benefit { get; set; } = string.Empty;
    public string? Situation { get; set; } = string.Empty;
    public string? AssociateImagemUrl { get; set; } = string.Empty;

    public ICollection<OperationDto>? Operations { get; set; } = [];
}