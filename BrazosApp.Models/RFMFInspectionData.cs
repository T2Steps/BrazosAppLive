using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models
{
    public class RFMFInspectionData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InspectionId { get; set; }
        [ForeignKey("InspectionId")]
        public Inspection? Inspection { get; set; }

        public int NumberOfRepeatedVio { get; set; }
        public int NumberOfVioCos { get; set; }

        public int Score { get; set; }
        //public string? Comment { get; set; }

        public bool SampleCollected { get; set; }

        //public int InspectedBy { get; set; }

        //public string? InspectedBySign { get; set; }

        //public string? InspectorSignFile { get; set; }

        public string? PersonInCharge { get; set; }
        public string? PersonInChargeSign { get; set; }

        [StringLength(50)]
        public string? CFM { get; set; }

        public DateTime? CFMExpiryDate { get; set; }

        [StringLength(50)]
        public string? FHC { get; set; }

        [StringLength(5)]
        public string? NumberOfEmployees { get; set; }

        public string? SecondaryInspector { get; set; }
        public string? SecondaryInspectorSign { get; set; }
        public string? SecondaryInspectorSignFile { get; set; }

    }
}
