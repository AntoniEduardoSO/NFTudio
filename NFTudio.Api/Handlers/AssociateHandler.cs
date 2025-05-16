using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NFTudio.Api.Data;
using NFTudio.Core.Handlers;
using NFTudio.Core.Models;
using NFTudio.Core.Requests.Associate;
using NFTudio.Core.Responses;

namespace NFTudio.Api.Handlers;
public class AssociateHandler(AppDbContext context) : IAssociateHandler
{
    public class StringUtils
    {
        public static string RemoveAccents(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string normalized = input.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }

    public async Task<Response<AssociateResponseDto?>> CreateAsync(CreateAssociateRequest request)
    {
        try
        {
            var associate = new Associate
            {
                Name = request.Name,
                Description = request.Description,
                Email = request.Email,
                Benefit = request.Benefit,
                Situation = request.Situation,
                AssociateImagemUrl = request.AssociateImagemUrl,
                Links = request.Links.Select(link => new Link
                {
                    Name = link.Name,
                    Type = link.Type
                }).ToList()
            };

            var associateOperations = new List<AssociateOperation>();

            foreach (var op in request.Operations)
            {
                var existingOperation = await context.Operations.FirstOrDefaultAsync(o => o.Name == op.Name);

                Operation operationToUse = existingOperation ?? new Operation { Name = op.Name };

                if (existingOperation == null)
                    context.Operations.Add(operationToUse);

                associateOperations.Add(new AssociateOperation
                {
                    Associate = associate,
                    Operation = operationToUse
                });
            }

            associate.AssociateOperations = associateOperations;

            await context.Associates.AddAsync(associate);
            await context.SaveChangesAsync();

            var responseDto = new AssociateResponseDto
            {
                Name = associate.Name,
                Description = associate.Description,
                Email = associate.Email,
                Benefit = associate.Benefit,
                Situation = associate.Situation,
                AssociateImagemUrl = associate.AssociateImagemUrl,
                Links = associate.Links.Select(l => new LinkDto { Name = l.Name, Type = l.Type }).ToList(),
                Operations = associate.AssociateOperations.Select(ao => ao.Operation.Name).ToList()
            };

            return new Response<AssociateResponseDto?>(responseDto, 201, "Empresa parceira adicionada com sucesso!");
        }
        catch
        {
            return new Response<AssociateResponseDto?>(null, 500, "Não foi possivel cadastrar empresa parceira");
        }
    }

    public async Task<PagedResponse<ICollection<AssociateResponseDto>>> GetAllHomeAsync(GetAllAssociateHomeRequest request)
    {
        try
        {
            var associatesList = await context.Associates
                .Include(a => a.AssociateOperations).ThenInclude(ao => ao.Operation)
                .Include(a => a.Links)
                .AsNoTracking()
                .Where(a => a.Situation == "Parceria Concluída e Publicada")
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = StringUtils.RemoveAccents(request.Search.ToLower());

                associatesList = associatesList
                    .Where(a =>
                        StringUtils.RemoveAccents(a.Name.ToLower()).Contains(search) ||
                        a.AssociateOperations.Any(ao =>
                            StringUtils.RemoveAccents(ao.Operation.Name.ToLower()).Contains(search)))
                    .ToList();
            }

            if (request.OperationNames?.Count > 0)
            {
                var tagSet = request.OperationNames
                  .Select(t => StringUtils.RemoveAccents(t.ToLower()))
                  .ToHashSet();
                  
                associatesList = associatesList
                    .Where(a =>
                        a.AssociateOperations
                         .Select(ao => StringUtils.RemoveAccents(ao.Operation.Name.ToLower()))
                         .Any(tagSet.Contains))
                    .ToList();
            }

            var totalCount = associatesList.Count;

            var paged = associatesList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();


            var dto = paged.Select(a => new AssociateResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Email = a.Email,
                Benefit = a.Benefit,
                Situation = a.Situation,
                AssociateImagemUrl = a.AssociateImagemUrl,
                Operations = a.AssociateOperations?.Select(ao => ao.Operation.Name).ToList() ?? [],
                Links = a.Links?.Select(l => new LinkDto { Name = l.Name, Type = l.Type }).ToList() ?? []
            }).ToList();

            return new PagedResponse<ICollection<AssociateResponseDto>>(
                dto,
                totalCount,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<ICollection<AssociateResponseDto>>(null, 500, "Não foi possível recuperar as empresas parceiras");
        }
    }

    public async Task<Response<AssociateResponseDto>> UpdateAsync(UpdateAssociateRequest request)
    {
        var associate = await context.Associates
            .Include(a => a.Links)
            .Include(a => a.AssociateOperations)
                .ThenInclude(ao => ao.Operation)
            .FirstOrDefaultAsync(a => a.Id == request.Id);
        
        if (associate == null)
        {
            return new Response<AssociateResponseDto>(null, 404, "Empresa nao encotrado");
        }

        associate.Name               = request.Name;
        associate.Description        = request.Description;
        associate.Email              = request.Email;
        associate.Benefit            = request.Benefit;
        associate.Situation          = request.Situation;
        associate.AssociateImagemUrl = request.AssociateImagemUrl;

        var linksToRemove = associate.Links
            .Where(l => !request.Links.Any(r => r.Id == l.Id)).ToList();
        context.Links.RemoveRange(linksToRemove);

        foreach (var lDto in request.Links)
        {
            var link = associate.Links.FirstOrDefault(x => x.Id == lDto.Id);
            if (link == null)                      
                associate.Links.Add(new Link { Name = lDto.Name, Type = lDto.Type });
            else                                    
            {
                link.Name = lDto.Name;
                link.Type = lDto.Type;
            }
        }

        var desired = request.Operations.Select(o => o.Name).ToHashSet();
        var toRemove = associate.AssociateOperations
            .Where(ao => !desired.Contains(ao.Operation.Name))
            .ToList();
        context.AssociateOperations.RemoveRange(toRemove);

        foreach (var opName in desired)
        {
            if (associate.AssociateOperations.All(ao => ao.Operation.Name != opName))
            {
                var op = await context.Operations.FirstOrDefaultAsync(o => o.Name == opName)
                         ?? new Operation { Name = opName };
                associate.AssociateOperations.Add(new AssociateOperation { Operation = op });
            }
        }

        await context.SaveChangesAsync();

        var dto = new AssociateResponseDto
        {
            Name        = associate.Name,
            Description = associate.Description,
            Email       = associate.Email,
            Benefit     = associate.Benefit,
            Situation   = associate.Situation,
            AssociateImagemUrl = associate.AssociateImagemUrl,
            Links       = associate.Links.Select(l => new LinkDto { Name = l.Name, Type = l.Type }).ToList(),
            Operations  = associate.AssociateOperations.Select(ao => ao.Operation.Name).ToList()
        };

        return new Response<AssociateResponseDto>(dto, 200, "Empresa atualizada com sucesso!");
    }

    public async Task<PagedResponse<ICollection<AssociateResponseDto>>> GetAllManageAsync(GetAllAssociateManageRequest request)
    {
        try
        {
            var associatesList = await context.Associates
                .Include(a => a.AssociateOperations).ThenInclude(ao => ao.Operation)
                .Include(a => a.Links)
                .AsNoTracking()
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = StringUtils.RemoveAccents(request.Search.ToLower());

                associatesList = associatesList
                    .Where(a =>
                        StringUtils.RemoveAccents(a.Name.ToLower()).Contains(search) ||
                        a.AssociateOperations.Any(ao =>
                            StringUtils.RemoveAccents(ao.Operation.Name.ToLower()).Contains(search)))
                    .ToList();
            }

            if(!string.IsNullOrWhiteSpace(request.Situation))
            {
                associatesList = associatesList
                    .Where(a => a.Situation == request.Situation)
                    .ToList();
            }

            if (request.OperationNames?.Count > 0)
            {
                var tagSet = request.OperationNames
                  .Select(t => StringUtils.RemoveAccents(t.ToLower()))
                  .ToHashSet();
                  
                associatesList = associatesList
                    .Where(a =>
                        a.AssociateOperations
                         .Select(ao => StringUtils.RemoveAccents(ao.Operation.Name.ToLower()))
                         .Any(tagSet.Contains))
                    .ToList();
            }

            var totalCount = associatesList.Count;

            var paged = associatesList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();


            var dto = paged.Select(a => new AssociateResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Email = a.Email,
                Benefit = a.Benefit,
                Situation = a.Situation,
                AssociateImagemUrl = a.AssociateImagemUrl,
                Operations = a.AssociateOperations?.Select(ao => ao.Operation.Name).ToList() ?? [],
                Links = a.Links?.Select(l => new LinkDto { Name = l.Name, Type = l.Type }).ToList() ?? []
            }).ToList();

            return new PagedResponse<ICollection<AssociateResponseDto>>(
                dto,
                totalCount,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<ICollection<AssociateResponseDto>>(null, 500, "Não foi possível recuperar as empresas parceiras");
        }
    }

    public async Task<Response<AssociateResponseDto>> DeleteAsync(DeleteAssociateRequest request)
    {
        var associate = await context
            .Associates
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (associate == null)
            return new Response<AssociateResponseDto>(null, 404, "Empresa parceira não encontrada.");

        context.Associates.Remove(associate);

        await context.SaveChangesAsync();

        return new Response<AssociateResponseDto>(null, message: "Empresa parceira excluída com sucesso!");
    }
}
