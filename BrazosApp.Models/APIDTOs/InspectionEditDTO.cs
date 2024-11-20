using BrazosApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.Models.APIDTOs
{
    public class InspectionEditDTO
    {
        public Inspection? Inspection { get; set; }

        public RFMFInspectionData? InspectionData { get; set; }

        public InspectionItemDetails? Detail { get; set; }

        public TemperatureObservation? Temperature { get; set; }

        public List<SelectListItem>? Purposes { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }

        public IEnumerable<Section>? Sections { get; set; }
        public IEnumerable<SubSection>? Subsections { get; set; }
        public IEnumerable<Item>? Items { get; set; }

        public IEnumerable<InspectionItemDetails>? Details { get; set; }

        public IEnumerable<TemperatureObservation>? TemperatureObservations { get; set; }

        public List<TemperatureObservation>? FinalTemperatureList { get; set; }
        public List<Item>? FinalItemList { get; set; }
        
        [StringLength(60)]
        public string? TimeIn { get; set; }
        [StringLength(60)]
        public string? TimeOut { get; set; }

        [StringLength(60)]
        public string? InspectedBy { get; set; }
    }

    public class InspectionEditSaveDTO
    {
        public InspectionEditDTO? Request { get; set; }
        public InspectionRequestDTO? inspection { get; set; }
        public InspectionItemDetails? InspectionItem { get; set; }
        public TemperatureObservation? TemperatureObservation { get; set; }
    }
}
