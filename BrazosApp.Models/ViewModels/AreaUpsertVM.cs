using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
      public class AreaUpsertVM
      {
            public int AreaId { get; set; }

            public string? Description { get; set; }

            public Area? Area { get; set; }

            public List<SelectListItem>? InspectorList { get; set; }

            public AreaWiseInspectors? AreaWiseInspectors { get; set; }
      }
}
