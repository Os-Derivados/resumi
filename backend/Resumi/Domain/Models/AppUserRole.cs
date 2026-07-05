using Microsoft.AspNetCore.Identity;

namespace Resumi.Domain.Models;

public class AppUserRole : IdentityUserRole<int>
{
    ICollection<IdentityRole<int>>? Roles { get; set; }
}