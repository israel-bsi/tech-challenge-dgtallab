using System.ComponentModel.DataAnnotations;

namespace TechChallengeDgtallab.Core.Requests.Department;

public class CreateDepartmentRequest
{
    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    public SuperiorDepartmentRequest? SuperiorDepartment { get; set; }
    public int? SuperiorDepartmentId { get; set; }

    public override string ToString() => Name;
}