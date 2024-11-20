using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
      public class InspectorReportVM
      {
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }

            public string? Inspector { get; set; }
            public string? SearchBy { get; set; }
            public List<SelectListItem>? UserList { get; set; }
      }
}
