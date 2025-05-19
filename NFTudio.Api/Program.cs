using NFTudio.Api;
using NFTudio.Api.Common;
using NFTudio.Api.Endpoints;
using Microsoft.AspNetCore.HttpOverrides;


var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddSecurity();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();


app.ConfigureDevEnvironment();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
    // Render usa IPs dinâmicos, então limpe as listas para confiar em todos
    KnownNetworks = { },  // vazio
    KnownProxies  = { }   // vazio
});


app.SeedSql();
await app.SeedUsersAsync();
app.UseHttpsRedirection();
app.UseCors(ApiConfiguration.CorsPolicyName);
app.UseSecurity();
app.MapEndpoints();

app.Run();
