using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class OnlineTFNewPermitVM
    {
        public int ApplicationId { get; set; }

        public int ApplicationForId { get; set; }

        public Establishment? Establishment { get; set; }

        public EstablishmentOwner? Owner { get; set; }

        public TFOperationDetails? OperationDetails { get; set; }

        public Document? Document { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }
        public string? ApplicationDt { get; set; }

        public string? EncryptedApplicationId { get; set; }
    }
}
