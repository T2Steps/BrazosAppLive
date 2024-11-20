using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class RFOperationDetails
    {
        public RFOperationDetails()
        {
            CreatedOn = DateTime.Now;
            IsActive = true;
        }
        [Key]
        public int Id { get; set; }

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

        public bool WithinCityLimitChoice { get; set; }

        public string? NumberOfEmployees { get; set; }

        //public string? PublicWaterSouce { get; set; }

        public int WaterSourceId { get; set; }
        [ForeignKey("WaterSourceId")]
        public WaterSource? WaterSource { get; set; }
        //public string? Publicsewage { get; set; }

        public int? PublicSewageId { get; set; }
        [ForeignKey("PublicSewageId")]
        public PublicSewage? PublicSewage { get; set; }
            //public string? PrivateWaterSeptic { get; set; }

            //public int? PrivateSepticId { get; set; }
            //[ForeignKey("PrivateSepticId")]
            //public PrivateSeptic? PrivateSeptic { get; set; }

        [StringLength(20)]
        public string? PrivateSeptic { get; set; }

        public string? CertifiedFoodManager { get; set; }

        public DateTime? CertificateExpirationDt { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public RFOperationDetails GetClone()
        {
            return (RFOperationDetails)this.MemberwiseClone();
        }
    }
}
