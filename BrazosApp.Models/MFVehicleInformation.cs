using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class MFVehicleInformation
      {
            public MFVehicleInformation()
            {
                  CreatedOn= DateTime.Now;
                  IsActive= true;
            }

            [Key]
            public int Id { get; set; }

            [Required]
            public int EstablishmentId { get; set; }
            [ForeignKey("EstablishmentId")]
            public Establishment? Establishment { get; set; }

            [StringLength(150)]
            public string? Make { get; set; }
            [StringLength(150)]
            public string? Model { get; set; }

            [StringLength(5)]
            public string? Year { get; set; }

            [StringLength(150)]
            public string? Color { get; set; }
            [StringLength(150)]
            public string? License { get; set; }
            [StringLength(150)]
            public string? VIN { get; set; }

            public bool IsActive { get; set; }
            public int CreatedBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int? UpdatedBy { get; set; }
            public DateTime? UpdatedOn { get; set; }

            public MFVehicleInformation GetClone()
            {
                  return (MFVehicleInformation)this.MemberwiseClone();
            }
      }
}
