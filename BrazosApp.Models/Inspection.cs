using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Inspection
    {
        public Inspection()
        {
            InspectionDate = DateTime.Now;
            //IsFollowUpInspection = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int EstablishmentId { get; set; }
        [ForeignKey("EstablishmentId")]
        public Establishment? Establishment { get; set; }

        [Required]
        public int RiskId { get; set; }
        [ForeignKey("RiskId")]
        public RiskCategory? RiskCategory { get; set; }

        [Required]
        public int PurposeId { get; set; }
        [ForeignKey("PurposeId")]
        public InspectionPurposes? Purpose { get; set; }

        public int ScheduleId { get; set; }
        [ForeignKey("ScheduleId")]
        public Schedule? Schedule { get; set; }

        public DateTime InspectionDate { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }

        public bool FollowUp { get; set; }
        public DateTime? FollowUpDate { get; set; }

        public int InspectedBy { get; set; }

        public string? InspectedBySign { get; set; }

        public string? InspectorSignFile { get; set; }

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public string? Comment { get; set; }

        public bool IsPermitSuspended { get; set; }

        //public bool IsFollowUpInspection { get; set; }

        public int? ParentInspectionId { get; set; }

        //public bool IsSync { get; set; }

        //public DateTime? SyncDate { get; set; }
    }
}
