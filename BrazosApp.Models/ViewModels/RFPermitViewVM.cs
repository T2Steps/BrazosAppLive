using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class RFPermitViewVM
    {
        public Establishment? Establishment { get; set; }

        public EstablishmentOwner? Owner { get; set; }

        public RFOperationDetails? OperationDetails { get; set; }

        public List<SelectListItem>? BusinessTypesEng { get; set; }
        public List<SelectListItem>? BusinessTypesSp { get; set; }

        public List<SelectListItem>? OperationTypesEng { get; set; }
        public List<SelectListItem>? PublicWaterSourceList { get; set; }
        public List<SelectListItem>? PublicSewageList { get; set; }
        public List<SelectListItem>? PrivateSeptic { get; set; }
        public List<SelectListItem>? OperationTypesSp { get; set; }
        public PlanReviewVM? planReviewVM { get; set; }

        public DateTime? ApplicationDate { get; set; } = new DateTime();

        public string? CreatedBy { get; set; }

        public string? CertExpiryDt { get; set; }

        public string? EncryptedEstId { get; set; }      

        public string? ApplicationNo { get; set; }
        
    }
}
