using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class AgencyStaffReqFields
    {
        public AgencyStaffReqFields()
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

        [Required(ErrorMessage = "Please select risk category.")]
        public int RiskCategoryId { get; set; }
        [ForeignKey("RiskCategoryId")]
        public RiskCategory? RiskCategory { get; set; }

        //[Required(ErrorMessage = "Please select area")]
        //[Range(1,8, ErrorMessage = "Please select area")]
        public int AreaId { get; set; }
        [ForeignKey("AreaId")]
        public Area? Area { get; set; }

        [Required(ErrorMessage = "Please select establishment size.")]
        public int EstablishmentSizeId { get; set; }
        [ForeignKey("EstablishmentSizeId")]
        public EstablishmentSize? EstablishmentSize { get; set; }

        [Required(ErrorMessage = "Please select establishment type.")]
        public int EstablishmentTypeId { get; set; }
        [ForeignKey("EstablishmentTypeId")]
        public EstablishmentTypes? EstablishmentTypes { get; set; }

        public bool IsPlanReview { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
