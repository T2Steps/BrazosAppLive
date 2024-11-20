using BrazosApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrazosAPI.Models.DTOs
{
      public class ScheduleModalDataRequestDTO
      {
            public List<SelectListItem>? Purposes { get; set; }
            public List<SelectListItem>? AssignInspectorList { get; set; }
            public int ScheduleId { get; set; }
            public int EstId { get; set; }
            public int PurposeId { get; set; }

            public int InspectorId {  get; set; }

            public DateTime ScheduledDate { get; set; }

            public InspectionSearchParamsVM? SearchParamsVM { get; set; }
      }

    public class ScheduleModalDataResponseDTO
    {
        public List<SelectListItem>? Purposes { get; set; }
        public List<SelectListItem>? AssignInspectorList { get; set; }

        public List<SelectListItem>? SearchPurposeList { get; set; }
        public List<SelectListItem>? SearchAssignInspectorList { get; set; }
    }


    public class AreaIDList
    {
         public int Id { get; set; }
    }
}
