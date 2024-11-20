using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class EditInspectionVM
    {
        public Inspection? Inspection { get; set; }

        public RFMFInspectionData? InspectionData { get; set; }

        public InspectionItemDetails? Detail { get; set; }

        public TemperatureObservation? Temperature { get; set; }

        public List<SelectListItem>? Purposes { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }

        public IEnumerable<Section>? Sections { get; set; }
        public IEnumerable<SubSection>? Subsections { get; set; }
        public IEnumerable<Item>? Items { get; set; }

        public IEnumerable<InspectionItemDetails>? Details { get; set; }

        public IEnumerable<TemperatureObservation>? TemperatureObservations { get; set; }

        public List<TemperatureObservation>? FinalTemperatureList { get; set; }
        public List<Item>? FinalItemList { get; set; }

        public InspectionRequestDTO? inspection { get; set; }
        public InspectionItemDetails? InspectionItem { get; set; }
        public TemperatureObservation? TemperatureObservation { get; set; }
        [StringLength(60)]
        public string? TimeIn { get; set; }
        [StringLength(60)]
        public string? TimeOut { get; set; }

        [StringLength(60)]
        public string? InspectedBy { get; set; }
    }

      public class InspectionRequestDTO
      {
            public int EstablishmentId { get; set; }

            public int? InspectionId { get; set; }

            public int ScheduleId { get; set; }

            public int PurposeId { get; set; }

            public int RiskId { get; set; }

            public DateTime InspectionDate { get; set; }

            public DateTime TimeIn { get; set; }
            public DateTime TimeOut { get; set; }

            public int Score { get; set; }

            public int NumberOfRepeatedVio { get; set; }
            public int NumberOfVioCos { get; set; }

            public bool SampleCollected { get; set; }

            public bool PermitSuspend { get; set; }

            public bool FollowUp { get; set; }
            public DateTime? FollowUpDate { get; set; }

            public string? InspectedBySign { get; set; }

            public string? InspectorSignFile { get; set; }

            public string? PersonInCharge { get; set; }
            public string? PersonInChargeSign { get; set; }

            public string? Comment { get; set; }
            public string? CFM { get; set; }
            public string? FHC { get; set; }
            public DateTime? CFMExpDate { get; set; }

            public string? NumberOfEmployees { get; set; }

            public string? SecondInspector { get; set; }
            public string? SecondInspectorSigns { get; set; }
            public string? SecondInspectorSignFile { get; set; }
      }

}
