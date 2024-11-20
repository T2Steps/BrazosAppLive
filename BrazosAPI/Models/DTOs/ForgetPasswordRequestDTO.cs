namespace BrazosAPI.Models.DTOs
{
      public class ForgetPasswordRequestDTO
      {
            public string? BHCD { get; set; }

            public string? EmailId { get; set; }

            public bool IamNotRobotChk { get; set; }
      }

      public class ForgetPasswordResponseDTO
      {
            public string? Username { get; set; }
            public string? TempPassword { get; set; }
            public string? UserEmail { get; set; }
      }
}
