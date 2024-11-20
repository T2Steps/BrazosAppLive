using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class OpeningInspectionDataDTO
    {
        public int EstablishmentId { get; set; }

        public int ScheduleId { get; set; }

        public int PurposeId { get; set; }

        public int RiskId { get; set; }

        public DateTime InspectionDate { get; set; }

        public DateTime TimeIn { get; set; }

        public bool FollowUp { get; set; }
        public DateTime? FollowUpDate { get; set; }

        public string? InspectedBySign { get; set; }

        public string? InspectorSignFile { get; set; }

        public string? PersonInCharge { get; set; }
        public string? PersonInChargeSign { get; set; }

        public string? Comment { get; set; }

        public bool PermitApproval { get; set; }

        public string? SecondInspector { get; set; }
        public string? SecondInspectorSigns { get; set; }
        public string? SecondInspectorSignFile { get; set; }
    }
}
