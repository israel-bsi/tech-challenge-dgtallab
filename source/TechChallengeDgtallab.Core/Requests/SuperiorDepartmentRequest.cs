namespace TechChallengeDgtallab.Core.Requests;

public class SuperiorDepartmentRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public override string ToString() => Name;
}