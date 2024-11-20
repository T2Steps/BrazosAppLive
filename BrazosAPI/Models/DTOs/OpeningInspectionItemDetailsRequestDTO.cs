namespace BrazosAPI.Models.DTOs
{
    public class OpeningInspectionItemDetailsRequestDTO
    {
        public int InspectionId { get; set; }

        public int ItemId { get; set; }

        public string? Status { get; set; }
    }
}
