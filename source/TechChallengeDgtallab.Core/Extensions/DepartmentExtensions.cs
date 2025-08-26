using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Requests.Collaborator;
using TechChallengeDgtallab.Core.Requests.Department;
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
                    Name = department.Manager.Name,
                    Cpf = department.Manager.Cpf
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
                Name = department.Manager.Name,
                Cpf = department.Manager.Cpf
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

    public static IEnumerable<UpdateDepartmentRequest> ToRequest(this IEnumerable<DepartmentResponse> response)
    {
        var requests = new List<UpdateDepartmentRequest>();
        foreach (var department in response)
        {
            var item = new UpdateDepartmentRequest
            {
                Id = department.Id,
                Name = department.Name,
                ManagerId = department.Manager?.Id,
                SuperiorDepartmentId = department.SuperiorDepartment?.Id,
                SuperiorDepartment = department.SuperiorDepartment is not null
                    ? new SuperiorDepartmentRequest
                    {
                        Id = department.SuperiorDepartment.Id,
                        Name = department.SuperiorDepartment.Name
                    }
                    : null,
                Manager = department.Manager is not null
                    ? new UpdateCollaboratorRequest
                    {
                        Id = department.Manager.Id,
                        Name = department.Manager.Name
                    }
                    : null,
            };
            requests.Add(item);
        }
        return requests;
    }

    public static IEnumerable<SuperiorDepartmentRequest> ToSuperiorRequest(this IEnumerable<DepartmentResponse> response)
    {
        var requests = new List<SuperiorDepartmentRequest>();
        foreach (var department in response)
        {
            var item = new SuperiorDepartmentRequest
            {
                Id = department.Id,
                Name = department.Name
            };
            requests.Add(item);
        }
        return requests;
    }

    public static IEnumerable<DepartmentDto> ToDto(this IEnumerable<DepartmentResponse> response)
    {
        var dtos = new List<DepartmentDto>();
        foreach (var department in response)
        {
            var item = new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name
            };
            dtos.Add(item);
        }
        return dtos;
    }

    public static Department ToEntity(this UpdateDepartmentRequest request)
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