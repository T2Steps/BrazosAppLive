using BrazosApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.DTOs
{
    public class OpeningInspectionCreateVM
    {
        public OpeningInspectionResponseDTO? Response { get; set; }

        public InspectionRequestDTO? Inspection { get; set; }
        public OpeningInspectionDataDTO? OpeningInspection { get; set; }

        public OpeningInspectionItemDetailsRequestDTO? InspectionItem { get; set; }
    }
}
