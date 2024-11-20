namespace BrazosApp.Models.DTOs
{
    public class RetailPermitListDTO
    {
        public int Id { get; set; }

        public string? PermitNumber { get; set; }
        public string? ApplicationNumber { get; set; }
        public string? Name { get; set; }
        public int Area { get; set; }

        public string? Risk { get; set; }
        public string? PermitStatus { get; set; }
        public string? ApplicationFor { get; set; }

        public string? Code { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime ApplicationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? NextInspectionDate { get; set; }
        public bool IsActive { get; set; }
        public string? EncryptedId { get; set; }

    }
}
