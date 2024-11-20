using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class MFOperationDetails
      {
            public MFOperationDetails()
            {
                  IsActive= true;
                  CreatedOn= DateTime.Now;
            }

            [Key]
            public int Id { get; set; }

            [Required]
            public int EstablishmentId { get; set; }
            [ForeignKey("EstablishmentId")]
            public Establishment? Establishment { get; set; }

            public int BusinessTypeId { get; set; }
            [ForeignKey("BusinessTypeId")]
            public BusinessType? BusinessType { get; set; }

            public int OperationTypeId { get; set; }
            [ForeignKey("OperationTypeId")]
            public OperationType? OperationType { get; set; }

            //public int WaterSourceId { get; set; }
            //[ForeignKey("WaterSourceId")]
            //public WaterSource? WaterSource { get; set; }
            [StringLength(150)]
            public string? CentralProcessingFacility { get; set; }

            public string? Street { get; set; }

            public string? State { get; set; }

            public string? City { get; set; }
            [StringLength(20)]
            public string? Zip { get; set; }

            //public string? MobileOperationLocation { get; set; }

            //public string? PublicWaterSouce { get; set; }

            public int WaterSourceId { get; set; }
            [ForeignKey("WaterSourceId")]
            public WaterSource? WaterSource { get; set; }

            public string? WasteWaterDisposalsite { get; set; }

            public string? Portablewatertanksize { get; set; }

            public string? Wastewatertanksize { get; set; }

            public string? CertifiedFoodManager { get; set; }

            public DateTime? CertificateExpirationDt { get; set; }

            public bool IsActive { get; set; }
            public DateTime CreatedOn { get; set; }

            public int CreatedBy { get; set; }

            public DateTime? UpdatedOn { get; set; }

            public int? UpdatedBy { get; set; }

            public MFOperationDetails GetClone()
            {
                  return (MFOperationDetails)this.MemberwiseClone();
            }
      }
}
