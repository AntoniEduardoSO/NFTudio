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
app.SeedSql();
await app.SeedUsersAsync();
app.UseHttpsRedirection();
app.UseCors(ApiConfiguration.CorsPolicyName);
app.UseSecurity();
app.MapEndpoints();

app.Run();
