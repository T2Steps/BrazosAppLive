using System.ComponentModel.DataAnnotations;

namespace BrazosAPI.Models.DTOs
{
      public class LoginRequestDTO
      {
            [Required]
            public string? BHCD { get; set; }

            [Required]
            public string? Password { get; set; }

            //public bool IsPersistent { get; set; }
      }
}
