using System.Collections.Generic;
using KickAround.Data;
using KickAround.Models.BindingModels.Games;
using KickAround.Models.EntityModels;
using KickAround.Models.ViewModels.Games;

namespace KickAround.Services.Contracts
{
    public interface IGamesService
    {
        void CreateGame(string id, CreateGameBindingModel bindingModel);
        IEnumerable<GameViewModel> GetAllGames(string userId);
        GameDetailsViewModel GetGameDetails(string id, string userId);
        EditGameBindingModel GetEditGameModel(Game game);
        void EditGame(EditGameBindingModel bindingModel);
        void DeleteGame(string id);
        Game GetGame(string id);
        bool IsUserAuthorizedToSeeGameDetails(string userId, Game game);
        bool IsUserAuthorizedToModifyGame(string userId, Game game);
        void JoinGame(string id, string userId);
        Game GetNextGame(string userId);
        void LeaveGame(string gameId, string userId);
        void CancelGame(string gameId, string userId);
        KickAroundContext Context { get; }
    }
}