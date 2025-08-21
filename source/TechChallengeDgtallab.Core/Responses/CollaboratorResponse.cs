using TechChallengeDgtallab.Core.Models;

namespace TechChallengeDgtallab.Core.Responses;

public class CollaboratorResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? Rg { get; set; }
    public DepartmentResponse? Department { get; set; }

    public static implicit operator CollaboratorResponse(Collaborator collaborator)
    {
        return new CollaboratorResponse
        {
            Id = collaborator.Id,
            Name = collaborator.Name,
            Cpf = collaborator.Cpf,
            Rg = collaborator.Rg,
            Department = collaborator.Department
        };
    }
}