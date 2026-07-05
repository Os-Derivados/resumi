using Microsoft.AspNetCore.Identity;

namespace Resumi.Domain.Models;

public class AppRole : IdentityRole<int>
{
    public ICollection<AppUserRole>? UserRoles { get; set; }
}