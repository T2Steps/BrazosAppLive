namespace BrazosApp.Models.DTOs
{
    public class InspectionItemDetailsRequestDTO
    {
        public int InspectionId { get; set; }

        public int ItemId { get; set; }

        public string? Status { get; set; }

        public string? Comment { get; set; }
        public string? Cos { get; set; }
        public string? R { get; set; }
        public string? ImageFile { get; set; }
    }
}
