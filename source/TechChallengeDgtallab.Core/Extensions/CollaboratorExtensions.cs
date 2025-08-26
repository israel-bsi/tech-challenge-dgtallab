using TechChallengeDgtallab.Core.Models;
using TechChallengeDgtallab.Core.Requests.Collaborator;
using TechChallengeDgtallab.Core.Requests.Department;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Core.Extensions;

public static class CollaboratorExtensions
{
    public static IEnumerable<CollaboratorResponse> ToResponse(this IEnumerable<Collaborator> collaborators)
    {
        return collaborators.Select(collaborator => new CollaboratorResponse
        {
            Id = collaborator.Id,
            Name = collaborator.Name,
            Cpf = collaborator.Cpf,
            Rg = collaborator.Rg,
            Department = new DepartmentInCollaboratorResponse
            {
                Id = collaborator.Department.Id,
                Name = collaborator.Department.Name
            }
        });
    }

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
            response.Department = new DepartmentInCollaboratorResponse
            {
                Id = collaborator.Department.Id,
                Name = collaborator.Department.Name
            };
        }

        return response;
    }

    public static IEnumerable<UpdateCollaboratorRequest> ToRequest(this IEnumerable<CollaboratorResponse> collaborators)
    {
        var requests = new List<UpdateCollaboratorRequest>();
        foreach (var collaborator in collaborators)
        {
            var request = new UpdateCollaboratorRequest
            {
                Id = collaborator.Id,
                Name = collaborator.Name,
                Cpf = collaborator.Cpf,
                Rg = collaborator.Rg,
                DepartmentId = collaborator.Department?.Id ?? 0,
                Department = collaborator.Department is not null
                    ? new UpdateDepartmentRequest
                    {
                        Id = collaborator.Department.Id,
                        Name = collaborator.Department.Name
                    }
                    : null
            };
            requests.Add(request);
        }
        return requests;
    }

    public static Collaborator ToEntity(this UpdateCollaboratorRequest request)
    {
        return new Collaborator
        {
            Id = request.Id,
            Name = request.Name,
            Cpf = request.Cpf,
            Rg = request.Rg,
            DepartmentId = request.DepartmentId,
            IsActive = true,
            UpdatedAt = DateTime.UtcNow,
        };
    }
}