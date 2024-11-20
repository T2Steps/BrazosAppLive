using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class OpeningInspectionData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InspectionId { get; set; }
        [ForeignKey("InspectionId")]
        public Inspection? Inspection { get; set; }

        public bool PermitApproval { get; set; }

        
        public string? PersonInCharge { get; set; }
        public string? PersonInChargeSign { get; set; }

        public string? SecondaryInspector { get; set; }
        public string? SecondaryInspectorSign { get; set; }
        public string? SecondaryInspectorSignFile { get; set; }
    }
}
