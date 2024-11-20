using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class Schedule
    {
        public Schedule()
        {
            CreatedOn = DateTime.Now;
            IsAutoSchedule = false;
            IsFollowUpSchedule = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int EstablishmentId { get; set; }
        [ForeignKey("EstablishmentId")]
        public Establishment? Establishment { get; set; }

        [Required]
        public int PurposeId { get; set; }
        [ForeignKey("PurposeId")]
        public InspectionPurposes? Purpose { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public ScheduleStatus? Status { get; set; }

        public int AssignedTo { get; set; }

        public DateTime ScheduledDate { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        //public bool IsSync { get; set; }

        public DateTime? SyncDate { get; set; }

        public bool IsAdhoc { get; set; }

        public bool IsAutoSchedule { get; set; }

        public bool IsFollowUpSchedule { get; set; }

        public int? ParentInspectionId { get; set; }
    }
}
