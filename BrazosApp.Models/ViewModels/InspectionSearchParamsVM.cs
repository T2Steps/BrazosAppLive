using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
      public class InspectionSearchParamsVM
      {
            public string? Name { get; set; }
            public string? Permit { get; set; }

            public string? Purpose { get; set; }

            public string? Address { get; set; }

            public string? Inspector { get; set; }

            public string? AssignedTo { get; set; }

            public string? FollowUp { get; set; }

            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public List<SelectListItem>? PurposeList { get; set; }

            public List<SelectListItem>? AssignInspectorList { get; set; }

            public List<SelectListItem>? UserList { get; set; }
      }
}
