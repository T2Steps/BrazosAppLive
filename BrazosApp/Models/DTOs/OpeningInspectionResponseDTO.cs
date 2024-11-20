using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BrazosApp.Models.DTOs
{
    public class OpeningInspectionResponseDTO
    {
            //public Inspection? Inspection { get; set; }
            //public InspectionItemDetails? detail { get; set; }
            //public IEnumerable<InspectionItemDetails>? details { get; set; }

            //public IEnumerable<Item>? items { get; set; }
            //public List<Item>? FinalItemList { get; set; }
            //public Users? users { get; set; }

            //public Schedule? Scheduler { get; set; }
            //public int? SID { get; set; }
            //public DateTime? InTime { get; set; }

            public int SId { get; set; }
            public int EstablishmentId { get; set; }

            public int PurposeId { get; set; }

            public int RiskId { get; set; }

            public string? EstName { get; set; }

            public string? Permit { get; set; }

            public string? Address { get; set; }

            public string? InspectedBy { get; set; }

            public string? InspectedBySignFileName { get; set; }

            [StringLength(5)]
            public string? Code { get; set; }

            public IEnumerable<Item>? Items { get; set; }
            public List<Item>? FinalItemList { get; set; }
            public IEnumerable<InspectionItemDetails>? details { get; set; }
            public DateTime? InTime { get; set; }

            public List<SelectListItem>? UserList { get; set; }

    }
}
