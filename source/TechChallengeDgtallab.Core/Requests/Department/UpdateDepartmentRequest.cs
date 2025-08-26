using System.ComponentModel.DataAnnotations;
using TechChallengeDgtallab.Core.DTOs;

namespace TechChallengeDgtallab.Core.Requests.Department;

public class UpdateDepartmentRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    [ValidateComplexType(ErrorMessage = "Gerente inválido")]
    public CollaboratorDto? Manager { get; set; }
    public int? ManagerId { get; set; }

    public DepartmentDto? SuperiorDepartment { get; set; }
    public int? SuperiorDepartmentId { get; set; }

    public override string ToString() => Name;
}