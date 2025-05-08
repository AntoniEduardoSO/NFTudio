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
        StringBuilder sb = new StringBuilder();

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

    public async Task<PagedResponse<ICollection<AssociateResponseDto>>> GetAllAsync(GetAllAssociateRequest request)
    {
        try
        {
            var query = context.Associates
                .Include(a => a.AssociateOperations).ThenInclude(ao => ao.Operation)
                .Include(a => a.Links)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Search))
            {
                var searchLower = StringUtils.RemoveAccents(request.Search.ToLower());

                query = query
                .Where(
                    a => StringUtils.RemoveAccents(a.Name.ToLower()).Contains(searchLower)
                    || a.AssociateOperations.Any(ao => StringUtils.RemoveAccents(ao.Operation.Name.ToLower()).Contains(searchLower))
                );
            }

            if (request.OperationNames != null && request.OperationNames.Any())
            {
                foreach (var opName in request.OperationNames)
                {
                    var opNameLower = StringUtils.RemoveAccents(opName.ToLower());
                    query = query.Where(a =>
                        a.AssociateOperations.Any(ao => StringUtils.RemoveAccents(ao.Operation.Name.ToLower()).Equals(opNameLower, StringComparison.OrdinalIgnoreCase))
                    );
                }
            }

            var totalCount = await query.CountAsync();

            var associates = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var associateDtos = associates.Select(associate => new AssociateResponseDto
            {
                Name = associate.Name,
                Description = associate.Description,
                Email = associate.Email,
                Benefit = associate.Benefit,
                Situation = associate.Situation,
                AssociateImagemUrl = associate.AssociateImagemUrl,
                Operations = associate.AssociateOperations?.Select(ao => ao.Operation.Name).ToList() ?? [],
                Links = associate.Links?.Select(link => new LinkDto
                {
                    Name = link.Name,
                    Type = link.Type
                }).ToList() ?? []
            }).ToList();

            return new PagedResponse<ICollection<AssociateResponseDto>>(
                associateDtos,
                totalCount,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new PagedResponse<ICollection<AssociateResponseDto>>(null, 500, "Não foi possível recuperar as empresas parceiras");
        }
    }

    public Task<Response<Associate>> GetByFilterAsync(GetByFilterAssociateRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Associate>> UpdateAsync(UpdateAssociateRequest request)
    {
        throw new NotImplementedException();
    }
}
