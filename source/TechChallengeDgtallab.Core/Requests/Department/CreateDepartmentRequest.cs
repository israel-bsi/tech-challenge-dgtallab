using System.ComponentModel.DataAnnotations;
using TechChallengeDgtallab.Core.DTOs;

namespace TechChallengeDgtallab.Core.Requests.Department;

public class CreateDepartmentRequest
{
    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    public DepartmentDto? SuperiorDepartment { get; set; }
    public int? SuperiorDepartmentId { get; set; }

    public override string ToString() => Name;
}