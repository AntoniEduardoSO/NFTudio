namespace NFTudio.Core.Models;
public class AssociateResponseDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Benefit { get; set; } = string.Empty;
    public string Situation { get; set; } = string.Empty;
    public string AssociateImagemUrl { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public List<string> Operations { get; set; } = [];
    public List<LinkDto> Links { get; set; } = [];
}
