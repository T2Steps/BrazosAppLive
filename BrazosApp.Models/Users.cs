using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class Users
      {
            public Users()
            {
                  CreatedOn = DateTime.Now;
            }

            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "ⓘ Required Field ")]
            public int RoleId { get; set; }

            [ForeignKey("RoleId")]
            public Role? Role { get; set; }

            [Required(ErrorMessage = "ⓘ Required Field ")]
            public string? FirstName { get; set; }

            [Required(ErrorMessage = "ⓘ Required Field ")]
            public string? LastName { get; set; }

            [Required(ErrorMessage = "ⓘ Required Field ")]
            [EmailAddress(ErrorMessage = "ⓘ Invalid EmailId")]
            public string? EmailId { get; set; }

            public byte[]? Salt { get; set; }

            [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string? Password { get; set; }

            [Required(ErrorMessage = "ⓘ Required Field ")]
            public string? BHCD { get; set; }

            public string? RegisteredSanitarian { get; set; }

            public string? SanitarianInTrain { get; set; }

            public string? DesignatedRepresentative { get; set; }

            public string? CertifiedPoolOperator { get; set; }

            public string? CertifiedPoolInspector { get; set; }

            [NotMapped]
            public IFormFile? SignFile { get; set; }
            public string? SignFileName { get; set; }

            public bool IsActive { get; set; }
            public bool IsDelete { get; set; }
            public bool IsLoggedIn { get; set; }
            public DateTime LastSeenTime { get; set; }
            public int CreatedBy { get; set; }
            public int? UpdatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime? UpdatedOn { get; set; }

            public DateTime? SyncDate { get; set; }

            [NotMapped]
            public string? EncryptedId { get; set; }

            public Users GetClone()
            {
                  return (Users)this.MemberwiseClone();
            }
      }
}
