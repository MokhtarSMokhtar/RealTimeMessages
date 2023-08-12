using DatingApi.Entities;
using DatingApi.Extentions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser:IdentityUser<int>
    {

        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set;} = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string Introduction { get; set; } = "test";
        public string LookingFor { get; set; } = "test";
        public string Interests { get; set; } = "test";
        public string City { get; set; }
        public string Country { get; set; }
        public List<Photo> Photos { get; set; } = new();
        public List<UserLike> LikedByUser { get; set; }
        public List<UserLike> LikedUsers { get; set; }

        public List<Message> messagesSent { get; set; }
        public List<Message> messagesReceived { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }

    }
}
