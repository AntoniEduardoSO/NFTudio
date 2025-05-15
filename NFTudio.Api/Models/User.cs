using Microsoft.AspNetCore.Identity;

namespace NFTudio.Api.Models;
public class User : IdentityUser<long>
{
    public ICollection<IdentityRole<long>>? Roles { get; set; }
}