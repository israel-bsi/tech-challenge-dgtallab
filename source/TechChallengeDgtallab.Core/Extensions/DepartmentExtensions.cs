using TechChallengeDgtallab.Core.DTOs;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Extensions;

public static class DepartmentExtensions
{
    public static DepartmentResponse ToResponse(this Department department)
    {
        var response = new DepartmentResponse
        {
            Id = department.Id,
            Name = department.Name
        };

        if (department.Manager is { Id: > 0 })
        {
            response.Manager = new CollaboratorDto
            {
                Id = department.Manager.Id,
                Name = department.Manager.Name,
                Cpf = department.Manager.Cpf, 
                Manager = department.Manager.Name,
                Department = department.Name,
                DepartmentId = department.Id,
                Rg = department.Manager.Rg ?? string.Empty
            };
        }

        if (department.SuperiorDepartment is { Id: > 0 })
        {
            response.SuperiorDepartment = new DepartmentDto
            {
                Id = department.SuperiorDepartment.Id,
                Name = department.SuperiorDepartment.Name,
                Manager = department.Manager?.Name ?? string.Empty,
                ManagerId = department.ManagerId
            };
        }
        return response;
    }

    public static DepartmentDto ToDto(this DepartmentResponse response)
    {
        var dto = new DepartmentDto
        {
            Id = response.Id,
            Name = response.Name,
            ManagerId = response.Manager?.Id ?? 0,
            Manager = response.Manager?.Name ?? string.Empty,
            SuperiorDepartment = response.SuperiorDepartment?.Name ?? string.Empty,
            SuperiorDepartmentId = response.SuperiorDepartment?.Id
        };
        return dto;
    }
}