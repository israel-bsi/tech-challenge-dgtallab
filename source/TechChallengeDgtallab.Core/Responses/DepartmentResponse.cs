namespace TechChallengeDgtallab.Core.Responses;

public class DepartmentResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ManagerInDepartmentResponse? Manager { get; set; }
    public DepartmentInCollaboratorResponse? SuperiorDepartment { get; set; }
}

public class DepartmentInCollaboratorResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}