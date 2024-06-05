using System.ComponentModel.DataAnnotations;

namespace judge.system.core.DTOs.Requests.Account
{
    public class ResetPasswordReq
    {
        [Required]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        // public string Email { get; set; }
        public string Token { get; set; }
    }
}
