namespace BrazosApp.Models.DTOs
{
      public class ForgetPasswordResponseDTO
      {
            public string? Username { get; set; }
            public string? TempPassword { get; set; }

            public string? UserEmail { get; set; }
      }
}
