﻿namespace BrazosApp.Models.DTOs
{
      public class LoginResponseDTO
      {
            public int UserId { get; set; }
            public string? EncryptedId { get; set; }
            public string? Name { get; set; }
            public string? Role { get; set; }
            //public string? Email { get; set; }
            public string? Token { get; set; }
      }
}