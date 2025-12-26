using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Resumi.App.Data.Models;

public class AppUser : IdentityUser<int>, ITrackable
{
    [Required] [StringLength(128)] public string FullName { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Resume> Resumes { get; set; } = [];
}
