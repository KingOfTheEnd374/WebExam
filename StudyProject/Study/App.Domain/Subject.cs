using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Subject : BaseEntityId
{
    public string Name { get; set; } = default!;
    
    public Guid SemesterId { get; set; }
    public Semester? Semester { get; set; } = default!;
}