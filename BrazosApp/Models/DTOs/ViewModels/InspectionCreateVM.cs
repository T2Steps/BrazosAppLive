namespace BrazosApp.Models.DTOs.ViewModels
{
      public class InspectionCreateVM
      {
            public InspectionResponseDTO? Response { get; set; }

            public InspectionRequestDTO? Inspection { get; set; }
            public InspectionItemDetails? InspectionItem { get; set; }
            public TemperatureObservation? TemperatureObservation { get; set; }

            public AutoCompleteDTO? AutoComplete { get; set; }
      }
}
