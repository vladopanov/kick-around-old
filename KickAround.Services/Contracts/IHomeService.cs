using KickAround.Data;
using KickAround.Models.BindingModels.Users;

namespace KickAround.Services.Contracts
{
    public interface IHomeService
    {
        void PopulateGames(string userId);
        bool HasCurrentUserGroups(string userId);
        void CreateFeedback(FeedbackBindingModel bindingModel, string userId);
        KickAroundContext Context { get; }
    }
}