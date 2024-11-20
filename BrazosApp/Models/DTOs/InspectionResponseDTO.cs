using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BrazosApp.Models.DTOs
{
    public class InspectionResponseDTO
    {
        public int SId { get; set; }

        public int InspectionId { get; set; }
        public int EstablishmentId { get; set; }

        public int PurposeId { get; set; }

        public int RiskId { get; set; }

        public string? EstName { get; set; }

        public string? Permit { get; set; }

        public string? Address { get; set; }

        public string? InspectedBy { get; set; }

        public string? InspectedBySignFileName { get; set; }

        public string? InspectorEmailId { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }

        public IEnumerable<Item>? Items { get; set; }
        public IEnumerable<Section>? Sections { get; set; }
        public IEnumerable<SubSection>? SubSections { get; set; }
        public List<Item>? FinalItemList { get; set; }
        public IEnumerable<TemperatureObservation>? TemperatureObs { get; set; }
        public List<TemperatureObservation>? FinalTemperatureList { get; set; }
        public IEnumerable<InspectionItemDetails>? details { get; set; }
        public DateTime InTime { get; set; }

        public string? TimeIn { get; set; }

        public bool PermitSuspend { get; set; }

        public List<SelectListItem>? UserList { get; set; }
    }
}
