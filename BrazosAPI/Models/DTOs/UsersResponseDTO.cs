using BrazosApp.Models;

namespace BrazosAPI.Models.DTOs
{
      public class UsersResponseDTO
      {
            public IEnumerable<Users>? Users { get; set; }
      }

      public class SignatureFileRequestDTO
      {
            public string? base64String { get; set; }

            public string? userId { get; set; }
      }
}
