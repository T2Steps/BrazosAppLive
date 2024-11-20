using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class TFPermitEditVM
    {
        public string? EncryptedEstId { get; set; }
        public Establishment? Establishment { get; set; }

        public EstablishmentOwner? Owner { get; set; }

        public TFOperationDetails? OperationDetails { get; set; }
        public Document? Document { get; set; }
        public Notes? Notes { get; set; }

        public PlanReviewVM? planReviewVM { get; set; }

        public string? ApplicationNo { get; set; }

        public ScheduleVM? ScheduleVM { get; set; }

        public AddPaymentDTO? AddPaymentDTO { get; set; }

        public OfflinePaymentVM? OfflinePaymentVM { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }
    }
}
