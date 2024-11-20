using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.APIDTOs
{
    public class InspectionCommentPdfDTO
    {
        public Inspection? Inspection { get; set; }
        public RFMFInspectionData? InspectionData { get; set; }
        public string? InspectedBy { get; set; }
        public string? SecondaryInspectedBy { get; set; }
    }
}
