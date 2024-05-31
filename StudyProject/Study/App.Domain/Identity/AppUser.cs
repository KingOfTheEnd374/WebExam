using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    [MinLength(3)]
    [MaxLength(16)]
    public string Username { get; set; } = default!;
    
    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
}