using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;

namespace DatingApi.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
