using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
      public class OnlineRFNewPermitVM
      {
            public int ApplicationId { get; set; }

            public int ApplicationForId { get; set; }

            public DateTime? ApplicationDate { get; set; }

            public Establishment? Establishment { get; set; }

            public EstablishmentOwner? Owner { get; set; }

            public RFOperationDetails? OperationDetails { get; set; }

            public Document? Document { get; set; }

            public List<SelectListItem>? BusinessTypesEng { get; set; }
            public List<SelectListItem>? BusinessTypesSp { get; set; }

            public List<SelectListItem>? OperationTypesEng { get; set; }
            public List<SelectListItem>? PublicWaterSourceList { get; set; }
            public List<SelectListItem>? PublicSewageList { get; set; }
            public List<SelectListItem>? PrivateSeptic { get; set; }
            public List<SelectListItem>? OperationTypesSp { get; set; }

            public string? CertExpiryDt { get; set; }
            public string? ApplicationDt { get; set; }

            public string? EncryptedApplicationId { get; set; }
      }
}
