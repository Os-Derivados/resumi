using Microsoft.AspNetCore.Identity;

namespace Resumi.Domain.Models;

public class AppRole : IdentityRole<int>
{
    public AppRole(string roleName) : base(roleName)
    {
    }

    public AppRole()
    {
    }

    public ICollection<AppUserRole>? UserRoles { get; set; }
}