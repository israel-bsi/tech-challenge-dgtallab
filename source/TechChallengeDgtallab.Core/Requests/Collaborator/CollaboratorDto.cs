namespace TechChallengeDgtallab.Core.Requests.Collaborator;

public class CollaboratorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}