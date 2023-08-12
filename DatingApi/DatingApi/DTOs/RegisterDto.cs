using System.ComponentModel.DataAnnotations;

namespace DatingApi.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(8,MinimumLength =4)]
        public string Username { get; set; }
        public string Interests { get; set; } = "test";
        public string Introduction { get; set; } = "test";
        [Required] public string KnownAs { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public DateTime? DateOfBirth { get; set; } // optional to make required work!
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
