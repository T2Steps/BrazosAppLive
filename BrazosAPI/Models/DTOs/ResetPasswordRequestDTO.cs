using System.ComponentModel.DataAnnotations;

namespace BrazosAPI.Models.DTOs
{
    public class ResetPasswordRequestDTO
    {
        [Required]
        public string? CurrentPassword { get; set; }
        [Required]
        public string? NewPassword { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
