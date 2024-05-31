using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class UserSubject : BaseEntityId, IDomainAppUser<AppUser>
{
    public int Grade { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; } = default!;
    
    public Guid SubjectId { get; set; }
    public Subject? Subject { get; set; } = default!;
}