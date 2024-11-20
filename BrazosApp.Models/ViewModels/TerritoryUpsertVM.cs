using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
      public class TerritoryUpsertVM
      {
            public int TerritoryId { get; set; }

            public string? Name { get; set; }

            public string? ColorCode { get; set; }

            public int DefaultInspectorId { get; set; }

            public List<SelectListItem>? InspectorList { get; set; }

            public TerritoryWiseInspectors? TerritoryWiseInspectors { get; set; }
      }
}
