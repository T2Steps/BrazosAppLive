using System.ComponentModel.DataAnnotations;

namespace BrazosApp.Models.DTOs
{
      public class LoginRequestDTO
      {
            //[Required]
            //public string? BHCD { get; set; }

            //[Required]
            //public string? Password { get; set; }

            //public bool IsPersistent { get; set; }

            [Required(ErrorMessage = "ⓘ Required Field")]
            [RegularExpression("^[0-9]{5}$",ErrorMessage = "ⓘ Invalid Input")]
            [StringLength(5)]
            public string? BHCD { get; set; }

            [Required(ErrorMessage = "ⓘ Required Field")]
            public string? Password { get; set; }
      }
}
