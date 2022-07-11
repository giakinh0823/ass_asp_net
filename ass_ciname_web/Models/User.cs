﻿using System.ComponentModel.DataAnnotations;

namespace ass_ciname_web.Models
{
    public class User
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
