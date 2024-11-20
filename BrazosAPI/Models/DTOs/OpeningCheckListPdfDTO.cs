namespace BrazosAPI.Models.DTOs
{
    public class OpeningCheckListPdfDTO
    {
        public OpeningInspectionDetails? InspectionDetails { get; set; }
        //public InspectionItems? InspectionItems { get; set; }

        public List<OpeningInspectionItems>? InspectionItemList { get; set; }

        public string? Code { get; set; }
    }


    public class OpeningInspectionDetails
    {
        public int Id { get; set; }

        public int EstId { get; set; }
        public string? EstName { get; set; }
        public string? Permit { get; set; }
        public string? Address { get; set; }
        public string? Date { get; set; }
        public string? InspectedBy { get; set; }
        public string? InspectedBySignFile { get; set; }
        public string? InspectedBySign { get; set; }
        public string? ReceivedBy { get; set; }
        public string? ReceivedBySign { get; set; }
        public string? PermitApproval { get; set; }

        public string? SecondInspector { get; set; }
        public string? SecondInspectorSigns { get; set; }
        public string? SecondInspectorSignFile { get; set; }

        public string? Comment { get; set; }
    }

    public class OpeningInspectionItems
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
    }
}
