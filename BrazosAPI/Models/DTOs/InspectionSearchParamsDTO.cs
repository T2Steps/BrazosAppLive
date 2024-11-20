using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrazosAPI.Models.DTOs
{
    public class InspectionSearchParamsDTO
    {
        public string? Name { get; set; }
        public string? Permit { get; set; }

        public string? Purpose { get; set; }

        public string? Address { get; set; }

        public string? AssignedTo { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<SelectListItem>? PurposeList { get; set; }

        public List<SelectListItem>? AssignInspectorList { get; set; }
    }
}
