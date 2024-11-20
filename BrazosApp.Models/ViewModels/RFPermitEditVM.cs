using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class RFPermitEditVM
    {
        //public int ApplicationId { get; set; }

        //public int ApplicationForId { get; set; }

        public DateTime? ApplicationDate { get; set; } = new DateTime();

        public string? CreatedBy { get; set; }

        public Establishment? Establishment { get; set; }

        public EstablishmentOwner? Owner { get; set; }

        public RFOperationDetails? OperationDetails { get; set; }

        public Document? Document { get; set; }
        public Notes? Notes { get; set; }

        public List<SelectListItem>? BusinessTypesEng { get; set; }

        public List<SelectListItem>? OperationTypesEng { get; set; }

        public List<SelectListItem>? PublicWaterSourceList { get; set; }
        public List<SelectListItem>? PublicSewageList { get; set; }
        public List<SelectListItem>? PrivateSeptic { get; set; }

        public string? CertExpiryDt { get; set; }

        public string? EncryptedEstId { get; set; }

        public PlanReviewVM? planReviewVM { get; set; }

        public string? ApplicationNo { get; set; }

        public ScheduleVM? ScheduleVM { get; set; }

        public AddPaymentDTO? AddPaymentDTO { get; set; }

        public OfflinePaymentVM? OfflinePaymentVM { get; set; }
    }

    public class AddPaymentDTO
    {
        public string? InvoiceNo { get; set; }

        public int EstId { get; set; }

        public string? code { get; set; }

        public decimal? LateFine { get; set; }

        public decimal? MiscelliniusFees { get; set; }

        public string? MiscelliniusFeesTitle { get; set; }

        public List<FeesDetailsTable>? FeesList { get; set; }

        public int FeesChoiceId { get; set; }
        public List<SelectListItem>? FeesListDropDownItems { get; set; }
    }

      public class ScheduleVM
      {
            public Schedule? Schedule { get; set; }

            public DateTime ScheduleDate { get; set; }

            public List<SelectListItem>? InspectionPurposes { get; set; }

            public List<SelectListItem>? InspectorList { get; set; }
      }

    public class OfflinePaymentVM
    {
        public int PaymentId { get; set; }
        public DateTime CollectionDate { get; set; }

        public int PaymentMethodId { get; set; }

        public string? BankName { get; set; }

        public string? ReferenceNumber { get; set; }

        public List<PaymentSplit>? PaymentSplit { get; set; }
        //public string? CardNumber { get; set; }
    }
}
