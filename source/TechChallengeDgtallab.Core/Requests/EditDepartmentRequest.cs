using System.ComponentModel.DataAnnotations;

namespace TechChallengeDgtallab.Core.Requests;

public class EditDepartmentRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O Nome é obrigatório")]
    public string Name { get; set; } = string.Empty;

    public EditCollaboratorRequest? Manager { get; set; }
    public int? ManagerId { get; set; }

    public SuperiorDepartmentRequest? SuperiorDepartment { get; set; }
    public int? SuperiorDepartmentId { get; set; }

    public override string ToString() => Name;
}