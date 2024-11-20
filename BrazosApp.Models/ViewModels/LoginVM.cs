using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
      public class LoginVM
      {
        public LoginRequestDTO? LoginRequestDTO { get; set; }

        public bool IsPersistent { get; set; }
      }


    public class LoginRequestDTO
    {
        //[Required]
        //public string? BHCD { get; set; }

        //[Required]
        //public string? Password { get; set; }

        //public bool IsPersistent { get; set; }

        [Required(ErrorMessage = "ⓘ Required Field")]
        [RegularExpression("^[0-9]{4,6}$", ErrorMessage = "ⓘ Invalid Input")]
        [StringLength(5)]
        public string? BHCD { get; set; }

        [Required(ErrorMessage = "ⓘ Required Field")]
        public string? Password { get; set; }
    }
}
