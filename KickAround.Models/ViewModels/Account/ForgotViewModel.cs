using System.ComponentModel.DataAnnotations;

namespace KickAround.Models.ViewModels.Account
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
