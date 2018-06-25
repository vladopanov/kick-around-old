using KickAround.Data;
using KickAround.Models.ViewModels.Users;

namespace KickAround.Services.Contracts
{
    public interface IUsersService
    {
        UserDetailsViewModel GetUserDetails(string id);
        KickAroundContext Context { get; }
    }
}