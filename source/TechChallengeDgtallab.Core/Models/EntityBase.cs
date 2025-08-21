namespace TechChallengeDgtallab.Core.Models;

public abstract class EntityBase
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}