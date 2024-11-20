namespace BrazosApp.Models.DTOs
{
    public class FoodInspectionPdfDTO
    {
        public InspectionDetails? InspectionDetails { get; set; }
        public List<InspectionItems>? InspectionItemList { get; set; }
        public List<TemperatureObservation>? TemperatureObservations { get; set; }
        public string? Code { get; set; }



    }

    public class InspectionDetails
    {
        public int Id { get; set; }

        public int EstId { get; set; }

        public int PurposeId { get; set; }
        public string? EstName { get; set; }
        public string? Permit { get; set; }
        public string? Address { get; set; }
        public string? Date { get; set; }

        public string? TimeIn { get; set; }
        public string? TimeOut { get; set; }

        public int Score { get; set; }
        public string? Risk { get; set; }
        public string? InspectedBy { get; set; }
        public string? InspectedBySignFile { get; set; }
        public string? InspectedBySign { get; set; }

        public string? InspectedByEmail { get; set; }
        public string? ReceivedBy { get; set; }
        public string? ReceivedBySign { get; set; }
        
        public bool PermitSuspend { get; set; }
        public bool FollowUp { get; set; }
        public bool SampleCollected { get; set; }

        public string? Comment { get; set; }

        public string? CFM { get; set; }
        public string? FHC { get; set; }
        public string? NumberOfEmployees { get; set; }
        public DateTime? CFMExpDate { get; set; }

        public string? SecondaryInspector { get; set; }
        public string? SecondaryInspectorSign { get; set; }
        public string? SecondaryInspectorSignFile { get; set; }
    }

    public class InspectionItems
    {
        public int Id { get; set; }
        public int ItemId { get; set; }

        public int? SubCategoryId { get; set; }
        public bool IsNA { get; set; }
        public bool IsNO { get; set; }
        public string? Name { get; set; }

        public int? Point { get; set; }
        public string? Status { get; set; }

        public string? CDI { get; set; }

        public string? R { get; set; }

        public string? Image { get; set; }

        public string? Comment { get; set; }

        public int? ItemNumber { get; set; }
    }
}
