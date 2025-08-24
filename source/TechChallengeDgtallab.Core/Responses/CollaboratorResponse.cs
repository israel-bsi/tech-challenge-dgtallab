namespace TechChallengeDgtallab.Core.Responses;

public class CollaboratorResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? Rg { get; set; }
    public DepartmentInCollaboratorResponse? Department { get; set; }
}

public class ManagerInDepartmentResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}