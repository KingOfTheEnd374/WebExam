using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Homework : BaseEntityId
{
    public string Name { get; set; } = default!;
    
    public Guid SubjectId { get; set; }
    public Subject? Subject { get; set; } = default!;
}