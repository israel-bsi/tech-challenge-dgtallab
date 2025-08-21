using Microsoft.EntityFrameworkCore;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Repositories;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;
using TechChallengeDgtallab.Infra.Data;

namespace TechChallengeDgtallab.Infra.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _dbContext;

    public DepartmentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<Department>> AddAsync(Department department)
    {
        await _dbContext.Departments.AddAsync(department);
        await _dbContext.SaveChangesAsync();

        return new Response<Department>(department);
    }

    public async Task<Response<Department>> UpdateAsync(Department department)
    {
        _dbContext.Departments.Update(department);
        await _dbContext.SaveChangesAsync();

        return new Response<Department>(department);
    }

    public async Task<Response<Department>> DeleteAsync(Department department)
    {
        _dbContext.Departments.Update(department);
        await _dbContext.SaveChangesAsync();

        return new Response<Department>();
    }

    public async Task<Response<Department>> GetByIdAsync(int id)
    {
        var department = await _dbContext
            .Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);

        return new Response<Department>(department);
    }

    public async Task<PagedResponse<IEnumerable<Department>>> GetAllAsync(PagedRequest request)
    {
        var query = _dbContext
            .Departments
            .AsNoTracking()
            .Where(d => d.IsActive);

        var count = await query.CountAsync();

        var departments = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResponse<IEnumerable<Department>>(departments, count, request.PageNumber, request.PageSize);
    }
}