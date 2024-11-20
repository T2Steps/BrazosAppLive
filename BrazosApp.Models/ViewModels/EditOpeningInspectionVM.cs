using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.ViewModels
{
    public class EditOpeningInspectionVM
    {
        public Inspection? Inspection { get; set; }
        public OpeningInspectionData? InspectionData { get; set; }
        public IEnumerable<Item>? Items { get; set; }
        public IEnumerable<InspectionItemDetails>? Details { get; set; }
        public List<Item>? FinalItemList { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }

        [StringLength(60)]
        public string? TimeIn { get; set; }
        [StringLength(60)]
        public string? TimeOut { get; set; }

        public string? InspectedBy { get; set; }

        public InspectionEditRequestDTO? InspectionEditRequestDTO { get; set; }

        public InspectionItemDetails? InspectionItem { get; set; }
    }

    public class InspectionEditRequestDTO
    {
        public Inspection? Inspection { get; set; }

        public OpeningInspectionData? InspectionData { get; set; }
    }
}
