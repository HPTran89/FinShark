﻿using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Account
{
    public class RegisterDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
