using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
      public class ForgetPasswordVM
      {
            //[StringLength(5)]
            [RegularExpression("^[0-9]{4,6}$", ErrorMessage = "ⓘ Invalid Input")]
            [Required(ErrorMessage = "ⓘ Required Field ")]
            public string? BHCD { get; set; }

            [Required(ErrorMessage = "ⓘ Required Field ")]
            [EmailAddress(ErrorMessage = "Invalid Email")]
            public string? EmailId { get; set; }

            public bool IamNotRobotChk { get; set; }
      }
}
