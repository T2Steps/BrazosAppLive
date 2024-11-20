using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class OnlineDayCareApplicationVM
    {
        public int ApplicationId { get; set; }

        //public string? Name { get; set; }
        public DateTime? ApplicationDate { get; set; }

        public int ApplicationForId { get; set; }
        public Establishment? Establishment { get; set; }

        public EstablishmentOwner? Owner { get; set; }

        public string? ApplicationDt { get; set; }

        public Document? Document { get; set; }

        public string? EncryptedApplicationId { get; set; }
        //public string? Description { get; set; }

    }
}
