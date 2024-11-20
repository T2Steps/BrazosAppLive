using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class RiskCategory
      {
            public int Id { get; set; }

            public string? Name { get; set; }
            [StringLength(5)]
            public string? Code { get; set; }
            public string? Description { get; set; }
      }
}
