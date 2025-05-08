using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NFTudio.Api.Data;
using NFTudio.Api.Handlers;
using NFTudio.Core;
using NFTudio.Core.Handlers;
using NFTudio.Core.Requests.Associate;

var builder = WebApplication.CreateBuilder(args);



Configuration.ConnectionString =
            builder
                .Configuration
                .GetConnectionString("DefaultConnection")
            ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(
            x => { x.UseSqlite(Configuration.ConnectionString); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(n => n.FullName); });

builder.Services.AddTransient<IAssociateHandler,AssociateHandler>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.MapPost("/associate", handler: async (CreateAssociateRequest request, IAssociateHandler handler) 
    => 
    {
        return await handler.CreateAsync(request);
    })
    .WithName("Associates: Create")
    .WithSummary("Cria uma nova empresa parceira")
    .WithDescription("Cria uma nova empresa parceira")
    .WithOrder(1);

app.MapGet("/associate", async (IAssociateHandler handler, 
    [FromQuery] int pageNumber = Configuration.DefaultPageNumber, 
    [FromQuery] int pageSize  = Configuration.DefaultPageSize,
    [FromQuery] string search = "",
    [FromQuery] string[]? operationNames = null)
 =>
 {

    var request = new GetAllAssociateRequest
    {
        PageNumber = pageNumber,
        PageSize = pageSize,
        Search = search,
        OperationNames = operationNames
    };

    return await handler.GetAllAsync(request);
 });

app.Run();
