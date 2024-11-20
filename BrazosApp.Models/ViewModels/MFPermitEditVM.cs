using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class MFPermitEditVM
    {
        public Establishment? Establishment { get; set; }

        public DateTime ApplicationDate { get; set; }

        public string? CreatedBy { get; set; }

        public EstablishmentOwner? Owner { get; set; }

        public MFOperationDetails? OperationDetails { get; set; }

        public MFVehicleInformation? VehicleInformation { get; set; }

        public Document? Document { get; set; }
        public Notes? Notes { get; set; }

        public List<SelectListItem>? PublicWaterSourceList { get; set; }
        public List<SelectListItem>? BusinessTypesEng { get; set; }

        public List<SelectListItem>? OperationTypesEng { get; set; }

        public string? CertExpiryDt { get; set; }

        public string? EncryptedEstId { get; set; }

        public PlanReviewVM? planReviewVM { get; set; }
                
        public string? ApplicationNo { get; set; }
        public ScheduleVM? ScheduleVM { get; set; }

        public AddPaymentDTO? AddPaymentDTO { get; set; }

        public OfflinePaymentVM? OfflinePaymentVM { get; set; }

        public IEnumerable<MFOperationLocations>? MFOperationLocations { get; set; }
        public List<MFOperationLocations>? MFOperationLocationsList { get; set; }
    }

      //public class ScheduleVM
      //{
      //      public Schedule? Schedule { get; set; }

      //      public DateTime ScheduleDate { get; set; }

      //      public List<SelectListItem>? InspectionPurposes { get; set; }

      //      public List<SelectListItem>? InspectorList { get; set; }
      //}
}
