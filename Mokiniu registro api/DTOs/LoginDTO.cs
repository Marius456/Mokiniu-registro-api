﻿using System.ComponentModel.DataAnnotations;

namespace Mokiniu_registro_api.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
