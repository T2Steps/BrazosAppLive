using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class PlanReviewDSVM
    {
        public string? Code { get; set; }
        public AgencyStaffReqFields? agencyStaffReqFields { get; set; }

        public List<SelectListItem>? RiskCategoryList { get; set; }

        public List<SelectListItem>? AreaList { get; set; }

        public List<SelectListItem>? EstablishmentSizeList { get; set; }

        public List<SelectListItem>? JurisdictionList { get; set; }

        public List<SelectListItem>? EstablishmentTypesList { get; set; }        
        
        //[Required]
        //public string? Quater { get; set; }
        //[Required]
        //[Range(typeof(decimal), "1", "50000", ErrorMessage = "Amount must be greater than {1}.")]
        //[Precision(18, 2)]
        //public decimal? amount { get; set; }
        //public int feesCount { get; set; }
    }
}
