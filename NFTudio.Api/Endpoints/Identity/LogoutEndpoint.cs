using Microsoft.AspNetCore.Identity;
using NFTudio.Api.Common;
using NFTudio.Api.Models;

namespace NFTudio.Api.Endpoints.Identity;

public class LogoutEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapPost("/logout", HandleAsync)
        .RequireAuthorization();


    private static async Task<IResult> HandleAsync(SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
}