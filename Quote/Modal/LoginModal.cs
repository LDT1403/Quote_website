using System.ComponentModel.DataAnnotations;

namespace Quote.Modal
{
    public class LoginModal
    {
        [Required(ErrorMessage = "Mail là  bắt buộc.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là  bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }
    }
}
