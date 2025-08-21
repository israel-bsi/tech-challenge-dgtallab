using Microsoft.EntityFrameworkCore;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Repositories;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;
using TechChallengeDgtallab.Infra.Data;

namespace TechChallengeDgtallab.Infra.Repositories;

public class CollaboratorsRepository : ICollaboratorRepository
{
    private readonly AppDbContext _dbContext;
    public CollaboratorsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<CollaboratorResponse>> AddAsync(EditCollaboratorRequest request)
    {
        try
        {
            var collaborator = new Collaborator
            {
                Name = request.Name,
                Cpf = request.Cpf,
                Rg = request.Rg,
                DepartmentId = request.DepartmentId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _dbContext.Collaborators.AddAsync(collaborator);
            await _dbContext.SaveChangesAsync();

            return new Response<CollaboratorResponse>(collaborator, 201, "Colaborador cadastrado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<CollaboratorResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<CollaboratorResponse>> UpdateAsync(EditCollaboratorRequest request)
    {
        try
        {
            var collaborator = await _dbContext
                .Collaborators
                .FirstOrDefaultAsync(c => c.Id == request.Id);
            if (collaborator is null)
                return new Response<CollaboratorResponse>(null, 404, "Colaborador não encontrado.");

            collaborator.Name = request.Name;
            collaborator.Cpf = request.Cpf;
            collaborator.Rg = request.Rg;
            collaborator.DepartmentId = request.DepartmentId;
            collaborator.UpdatedAt = DateTime.UtcNow;

            _dbContext.Collaborators.Update(collaborator);
            await _dbContext.SaveChangesAsync();

            return new Response<CollaboratorResponse>(collaborator, 200, "Colaborador atualizado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<CollaboratorResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<CollaboratorResponse>> DeleteAsync(int id)
    {
        try
        {
            var collaborator = await _dbContext
                .Collaborators
                .FirstOrDefaultAsync(c => c.Id == id);

            if (collaborator is null)
                return new Response<CollaboratorResponse>(null, 404, "Colaborador não encontrado.");

            collaborator.IsActive = false;
            collaborator.UpdatedAt = DateTime.UtcNow;

            _dbContext.Collaborators.Update(collaborator);
            await _dbContext.SaveChangesAsync();

            return new Response<CollaboratorResponse>(null, 204, "Colaborador deletado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<CollaboratorResponse>(null, 500, e.Message);
        }
    }

    public async Task<Response<CollaboratorResponse>> GetByIdAsync(int id)
    {
        try
        {
            var collaborator = await _dbContext
                .Collaborators
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            return collaborator is null 
                ? new Response<CollaboratorResponse>(null, 404, "Colaborador não encontrado.") 
                : new Response<CollaboratorResponse>(collaborator, 200, "Colaborador encontrado com sucesso!");
        }
        catch (Exception e)
        {
            return new Response<CollaboratorResponse>(null, 500, e.Message);
        }
    }

    public async Task<PagedResponse<IEnumerable<CollaboratorResponse>>> GetAllAsync(PagedRequest request)
    {
        try
        {
            var query = _dbContext
                .Collaborators
                .AsNoTracking()
                .Where(c => c.IsActive);

            var count = await query.CountAsync();

            var collaborators = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var collaboratorsResponse = collaborators.Select(c => new CollaboratorResponse 
            {
                Id = c.Id,
                Name = c.Name,
                Cpf = c.Cpf,
                Rg = c.Rg,
                Department = c.Department
            });

            return new PagedResponse<IEnumerable<CollaboratorResponse>>(collaboratorsResponse, count, request.PageNumber, request.PageSize);
        }
        catch (Exception e)
        {
            return new PagedResponse<IEnumerable<CollaboratorResponse>>(null, 500, e.Message);
        }
    }
}