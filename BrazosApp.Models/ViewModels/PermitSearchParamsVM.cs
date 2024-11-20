using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class PermitSearchParamsVM
    {
        public string? Name { get; set; }
        public string? Permit { get; set; }

        public string? Owner { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? EstType { get; set; }
        public string? ApplicationNo { get; set; }
        public int? Area { get; set; }
        public string? Risk { get; set; }

        public string? SearchBy { get; set; }
        public string? PermitStatus { get; set; }
        public string? Purpose { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<SelectListItem>? AreaList { get; set; }
        public List<SelectListItem>? RiskList { get; set; }
        public List<SelectListItem>? PermitStatList { get; set; }

        public List<SelectListItem>? EstTypeList { get; set; }
    }
}
