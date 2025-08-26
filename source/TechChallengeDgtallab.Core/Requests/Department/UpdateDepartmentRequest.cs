using System.ComponentModel.DataAnnotations;
using TechChallengeDgtallab.Core.Requests.Collaborator;

namespace TechChallengeDgtallab.Core.Requests.Department;

public class UpdateDepartmentRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    [ValidateComplexType(ErrorMessage = "Gerente inválido")]
    public UpdateCollaboratorRequest? Manager { get; set; }
    public int? ManagerId { get; set; }

    public SuperiorDepartmentRequest? SuperiorDepartment { get; set; }
    public int? SuperiorDepartmentId { get; set; }

    public override string ToString() => Name;
}