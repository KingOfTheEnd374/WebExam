using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Semester : BaseEntityId
{
    public string Name { get; set; } = default!;
}