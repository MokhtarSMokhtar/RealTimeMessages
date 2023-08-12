using API.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;

namespace DatingApi.Entities
{
    public class AppUserRole:IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
        
    }
}
