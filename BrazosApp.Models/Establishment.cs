using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Establishment
    {
        public Establishment()
        {
            CreatedOn = DateTime.Now;
            IsActive = true;
            IsAgencyApproved = false;
        }

        [Key]
        public int Id { get; set; }

        public string? RiskCategory { get; set; }

        public int Area { get; set; }
        //public int? RiskCategoryId { get; set; }
        //[ForeignKey("RiskCategoryId")]
        //public RiskCategory? RiskCategory { get; set; }

        //public int? TerritoryId { get; set; }
        //[ForeignKey("TerritoryId")]
        //public Territory? Territory { get; set; }

        [Required]
        public int ApplicationForId { get; set; }
        [ForeignKey("ApplicationForId")]
        public ApplicationFor? ApplicationFor { get; set; }


        public string? Name { get; set; }
        public string? PermitNumber { get; set; }

        public string? Address { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? ContactNo { get; set; }        

        //public string? CertifiedFoodManager { get; set; }
        //public DateTime? CertificateExpirationDt { get; set; }

        public string? ApplicantSign { get; set; }
        public DateTime? ApplicantSignDate { get; set; }


        public DateTime? ActivationDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime? NextInspectionDate { get; set; }
        public DateTime? LastInspectionDate { get; set; }

        [Required]
        public int PermitStatusId { get; set; }
        [ForeignKey("PermitStatusId")]
        public PermitStatus? PermitStatus { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int? OldEstId { get; set; }
        public int? OldPermitStatusId { get; set; }

        public int? ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public bool IsAgencyApproved { get; set; }

        [StringLength(15)]
        public string? OldPermitNumber { get; set; }

        //public bool IsSync { get; set; }
        public DateTime? SyncDate { get; set; }

        public string? Description { get; set; }

        [NotMapped]
        public string? EncryptedId { get; set; }


        //public virtual TFOperationDetails? Details { get; set; }

        public Establishment GetClone()
        {
            return (Establishment)this.MemberwiseClone();
        }
    }
}
