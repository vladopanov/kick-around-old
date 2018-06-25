using System.ComponentModel;

namespace KickAround.Models.ViewModels.Users
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("Profile Picture")]
        public string ImageUrl { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
