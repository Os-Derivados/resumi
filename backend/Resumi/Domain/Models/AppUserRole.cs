using Microsoft.AspNetCore.Identity;

namespace Resumi.Domain.Models;

public class AppUserRole : IdentityUserRole<int>
{
    public IdentityRole<int>? Role { get; set; }
}