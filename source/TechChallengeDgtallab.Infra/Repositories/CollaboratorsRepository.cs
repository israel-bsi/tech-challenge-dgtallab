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

    public async Task<Response<Collaborator>> AddAsync(Collaborator collaborator)
    {
        await _dbContext.Collaborators.AddAsync(collaborator);
        await _dbContext.SaveChangesAsync();

        return new Response<Collaborator>(collaborator);
    }

    public async Task<Response<Collaborator>> UpdateAsync(Collaborator collaborator)
    {
        _dbContext.Collaborators.Update(collaborator);
        await _dbContext.SaveChangesAsync();

        return new Response<Collaborator>(collaborator);
    }

    public async Task<Response<Collaborator>> DeleteAsync(Collaborator collaborator)
    {
        _dbContext.Collaborators.Update(collaborator);
        await _dbContext.SaveChangesAsync();

        return new Response<Collaborator>();
    }

    public async Task<Response<Collaborator>> GetByIdAsync(int id)
    {
        var collaborator = await _dbContext
            .Collaborators
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        return new Response<Collaborator>(collaborator);
    }

    public async Task<PagedResponse<IEnumerable<Collaborator>>> GetAllAsync(PagedRequest request)
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

        return new PagedResponse<IEnumerable<Collaborator>>(collaborators, count, request.PageNumber, request.PageSize);
    }
}