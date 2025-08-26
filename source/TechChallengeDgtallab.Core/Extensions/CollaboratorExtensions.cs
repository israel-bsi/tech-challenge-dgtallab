using TechChallengeDgtallab.Core.DTOs;
using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Extensions;

public static class CollaboratorExtensions
{
    public static CollaboratorResponse ToResponse(this Collaborator collaborator)
    {
        var response = new CollaboratorResponse
        {
            Id = collaborator.Id,
            Name = collaborator.Name,
            Cpf = collaborator.Cpf,
            Rg = collaborator.Rg
        };

        if (collaborator.Department is { Id: > 0 })
        {
            response.Department = new DepartmentDto
            {
                Id = collaborator.Department.Id,
                Name = collaborator.Department.Name,
                Manager = collaborator.Department.Manager?.Name ?? string.Empty,
                ManagerId = collaborator.Department.ManagerId
            };
        }

        return response;
    }

    public static CollaboratorDto ToDto(this CollaboratorResponse response)
    {
        return new CollaboratorDto
        {
            Id = response.Id,
            Name = response.Name,
            Cpf = response.Cpf,
            Rg = response.Rg ?? string.Empty,
            DepartmentId = response.Department?.Id ?? 0,
            Department = response.Department?.Name ?? string.Empty,
            Manager = response.Department?.Manager ?? string.Empty
        };
    }
}