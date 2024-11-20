using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
      public class InspectionItemAdjuncTable
      {
            [Key]
            public int Id { get; set; }

            public int InspectionItemId { get; set; }
            [ForeignKey("InspectionItemId")]
            public InspectionItemDetails? InspectionItem { get; set; }
            
            [StringLength(5)]
            public string? ApproveStatus { get; set; }
      }
}
