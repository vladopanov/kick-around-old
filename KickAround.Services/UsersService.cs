using System.Linq;
using AutoMapper;
using KickAround.Models.EntityModels;
using KickAround.Models.ViewModels.Users;
using KickAround.Services.Contracts;

namespace KickAround.Services
{
    public class UsersService : Service, IUsersService
    {
        public UserDetailsViewModel GetUserDetails(string id)
        {
            User user = this.Context.Users.Find(id);

            UserDetailsViewModel viewModel = Mapper.Map<User, UserDetailsViewModel>(user);
            if (user.UserImages.ToList().Count > 0)
            {
                viewModel.ImageUrl = user.UserImages.ToList()[0].ImageUrl;
            }
            else
            {
                viewModel.ImageUrl = null;
            }

            return viewModel;
        }

        public UserImage DeleteImage(string id)
        {
            UserImage userImage = this.Context.UserImages.Find(id);
            this.Context.UserImages.Remove(userImage);
            this.Context.SaveChanges();

            return userImage;
        }

        public void AddPhoneNumber(string number, string userId)
        {
            User user = this.Context.Users.Find(userId);
            user.PhoneNumber = number;
            this.Context.SaveChanges();
        }

        public bool HasUserProfilePicture(string userId)
        {
            User user = this.Context.Users.Find(userId);

            return user.UserImages.Count > 0;
        }
    }
}
