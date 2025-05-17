using Microsoft.EntityFrameworkCore;
using NFTudio.Api.Data;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Links;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Handlers;

public class LinkHandler(AppDbContext context) : ILinkHandler
{
    public async Task<Response<ICollection<LinkDto?>>> CreateAsync(CreateLinkRequest request)
    {
        var associate = await context.Associates.FirstOrDefaultAsync(x => x.Id == request.AssociateId);

        if (associate is null)
            return new Response<ICollection<LinkDto?>>(null, 404, "Empresa parceira nao encontrada");

        if (request.Links is null || request.Links.Count == 0)
            return new Response<ICollection<LinkDto?>>(null, 400, "Nenhum link informado");

        var entities = request.Links.Select(l => new Link
        {
            Name = l.Name,
            Type = l.Type,
            AssociateId = request.AssociateId
        }).ToList();

        await context.Links.AddRangeAsync(entities);
        await context.SaveChangesAsync();

        var result = entities.Select(e => new LinkDto
        {
            Id = e.Id,
            Name = e.Name,
            Type = e.Type
        }).ToList();

        return new Response<ICollection<LinkDto?>>(result!, 201, "Links adicionados com sucesso");
    }

    public async Task<Response<LinkDto?>> DeleteAsync(DeleteLinkRequest request)
    {
        var link = await context.Links.FirstOrDefaultAsync(x => x.Id == request.Id && x.AssociateId == request.AssociateId);

        if (link == null)
            return new Response<LinkDto?>(null, 404, "Link não encontrado");

        context.Remove(link);

        var dto = new LinkDto
        {
            Id = link.Id,
            Name = link.Name,
            Type = link.Type
        };

        return new Response<LinkDto?>(dto, message: "Link removidom com sucesso!");
    }

    public async Task<Response<LinkDto?>> UpdateAsync(UpdateLinkRequest request)
    {
        var link = await context.Links
        .AsTracking()
        .FirstOrDefaultAsync(l =>
            l.Id == request.Id &&
            l.AssociateId == request.AssociateId);

        if (link is null)
            return new Response<LinkDto?>(null, 404,"Link não encontrado para essa empresa parceira");

        if (!string.IsNullOrWhiteSpace(request.Name))
            link.Name = request.Name;

        if (!string.IsNullOrWhiteSpace(request.Type))
            link.Type = request.Type;

        await context.SaveChangesAsync();

        var dto = new LinkDto
        {
            Id = link.Id,
            Name = link.Name,
            Type = link.Type
        };

        return new Response<LinkDto?>(dto, 200, "Link atualizado com sucesso");
    }
}
