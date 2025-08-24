using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Extensions;

public static class DepartmentExtensions
{
    public static IEnumerable<DepartmentResponse> ToResponse(this IEnumerable<Department> departments)
    {
        var response = new List<DepartmentResponse>();
        foreach (var department in departments)
        {
            var item = new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name
            };
            if (department.Manager is { Id: > 0 })
                item.Manager = new ManagerInDepartmentResponse
                {
                    Id = department.Manager.Id,
                    Name = department.Manager.Name
                };

            if (department.SuperiorDepartment is not null && department.SuperiorDepartment.Id > 0)
                item.SuperiorDepartment = new DepartmentInCollaboratorResponse
                {
                    Id = department.SuperiorDepartment.Id,
                    Name = department.SuperiorDepartment.Name
                };

            response.Add(item);
        }

        return response;
    }

    public static DepartmentResponse ToResponse(this Department department)
    {
        var response = new DepartmentResponse
        {
            Id = department.Id,
            Name = department.Name
        };

        if (department.Manager is { Id: > 0 })
        {
            response.Manager = new ManagerInDepartmentResponse
            {
                Id = department.Manager.Id,
                Name = department.Manager.Name
            };
        }

        if (department.SuperiorDepartment is { Id: > 0 })
        {
            response.SuperiorDepartment = new DepartmentInCollaboratorResponse
            {
                Id = department.SuperiorDepartment.Id,
                Name = department.SuperiorDepartment.Name
            };
        }
        return response;
    }

    public static Department ToEntity(this EditDepartmentRequest request)
    {
        return new Department
        {
            Id = request.Id,
            Name = request.Name,
            SuperiorDepartmentId = request.SuperiorDepartmentId,
            SuperiorDepartment = null,
            ManagerId = request.ManagerId,
            Manager = null,
            IsActive = true,
            UpdatedAt = DateTime.UtcNow
        };
    }
}