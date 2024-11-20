namespace BrazosAPI.Models.DTOs
{
    public class InspectionRequestDTO
    {
        //public InspectionRequestDTO()
        //{
        //    PermitApproval = 
        //}
        public int EstablishmentId { get; set; }

        public int ScheduleId { get; set; }

        public int PurposeId { get; set; }

        public int RiskId { get; set; }

        public DateTime InspectionDate { get; set; }

        public DateTime TimeIn { get; set; }

        public int Score { get; set; }

        public int NumberOfRepeatedVio { get; set; }
        public int NumberOfVioCos { get; set; }

        public bool SampleCollected { get; set; }

        public bool FollowUp { get; set; }
        public DateTime? FollowUpDate { get; set; }

        public string? InspectedBySign { get; set; }

        public string? InspectorSignFile { get; set; }

        public string? PersonInCharge { get; set; }
        public string? PersonInChargeSign { get; set; }

        public string? Comment { get; set; }

        public bool PermitApproval { get; set; }
    }
}
