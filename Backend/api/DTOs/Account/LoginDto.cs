using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Account
{
    public class LoginDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
