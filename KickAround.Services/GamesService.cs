using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using KickAround.Models.BindingModels.Games;
using KickAround.Models.EntityModels;
using KickAround.Models.EntityModels.Notifications;
using KickAround.Models.ViewModels.Games;
using KickAround.Services.Contracts;

namespace KickAround.Services
{
    public class GamesService : Service, IGamesService
    {
        public void CreateGame(string id, CreateGameBindingModel bindingModel)
        {
            Game game = Mapper.Map<CreateGameBindingModel, Game>(bindingModel);
            game.Id = Guid.NewGuid().ToString();
            Group group = this.Context.Groups.Find(id);
            game.Group = group;
            game.IsPattern = false;

            if (game.IsWeekly)
            {
                game.IsPattern = true;
                PopulateWeeklyGames(game, group);
            }

            this.Context.Games.Add(game);
            this.Context.SaveChanges();
        }

        private void PopulateWeeklyGames(Game game, Group group)
        {
            var month = game.Start.Value.Month + 1;
            var startDate = game.Start.Value.AddDays(7);
            var endDate = game.End.Value.AddDays(7);

            List<Game> games = new List<Game>();

            while (startDate.Month <= month)
            {
                Game currentGame = new Game()
                {
                    Id = Guid.NewGuid().ToString(),
                    IsWeekly = true,
                    Start = startDate,
                    End = endDate,
                    Group = group
                };
                games.Add(currentGame);

                startDate = startDate.AddDays(7);
                endDate = endDate.AddDays(7);
            }

            this.Context.Games.AddRange(games);
            this.Context.SaveChanges();
        }

        public IEnumerable<GameViewModel> GetAllGames(string userId)
        {
            IEnumerable<Group> groups = this.Context.Users
                .Include(u => u.GroupsPlaying.Select(gp => gp.Games)).SingleOrDefault(u => u.Id == userId)?.GroupsPlaying;

            List<GameViewModel> games = new List<GameViewModel>();
            if (groups != null)
                foreach (var group in groups)
                {
                    foreach (var game in group.Games)
                    {
                        GameViewModel vm = new GameViewModel()
                        {
                            Id = game.Id,
                            GroupName = group.Name,
                            Start = game.Start,
                            End = game.End,
                            IsCanceled = game.IsCanceled,
                            IsWeekly = game.IsWeekly
                        };

                        games.Add(vm);
                    }
                }

            return games;
        }

        public GameDetailsViewModel GetGameDetails(string id, string userId)
        {
            Game game = this.GetGame(id);

            GameDetailsViewModel gameDetails = Mapper.Map<Game, GameDetailsViewModel>(game);
            gameDetails.IsJoined = game.Players.Select(p => p.UserId).Contains(userId);
            gameDetails.IsAdmin = game.Group.Admins.Select(a => a.Id).Contains(userId);

            gameDetails.Players = gameDetails.Players.OrderBy(p => p.CreatedOn).ToList();

            return gameDetails;
        }

        public EditGameBindingModel GetEditGameModel(Game game)
        {
            EditGameBindingModel bindingModel = new EditGameBindingModel();


            return bindingModel;
        }

        public void EditGame(EditGameBindingModel bindingModel)
        {
            Game game = this.Context.Games.Find(bindingModel.Id);

            if (game != null)
            {
                game.Start = bindingModel.Start;
                game.End = bindingModel.End;

                this.Context.SaveChanges();
            }
        }

        public void DeleteGame(string id)
        {
            Game game = this.Context.Games.Find(id);

            if (game.IsWeekly)
            {
                IEnumerable<Game> games = this.Context.Games.AsEnumerable()
                    .Where(g => g.IsWeekly && g.Start > DateTime.Now
                    && g.Start.Value.DayOfWeek == game.Start.Value.DayOfWeek
                    && g.Start.Value.Hour == game.Start.Value.Hour
                    && g.Group == game.Group);

                IEnumerable<Game> previousGames = this.Context.Games.AsEnumerable()
                    .Where(g => g.IsWeekly && g.Start < DateTime.Now
                                && g.Start.Value.DayOfWeek == game.Start.Value.DayOfWeek
                                && g.Start.Value.Hour == game.Start.Value.Hour
                                && g.Group == game.Group);

                foreach (var previousGame in previousGames)
                {
                    previousGame.IsWeekly = false;
                }

                foreach (var g in games)
                {
                    IEnumerable<UserGame> usergames = this.Context.UserGames.Where(ug => ug.GameId == g.Id);
                    this.Context.UserGames.RemoveRange(usergames);
                }

                this.Context.Games.RemoveRange(games);
            }
            else
            {
                IEnumerable<UserGame> usergames = this.Context.UserGames.Where(ug => ug.GameId == game.Id);
                this.Context.UserGames.RemoveRange(usergames);
                this.Context.Games.Remove(game);
            }
            this.Context.SaveChanges();
        }

        public Game GetGame(string id)
        {
            Game game = this.Context.Games
                .Include(g => g.Group.Players)
                .Include(g => g.Group.Admins)
                .Include(g => g.Players)
                .SingleOrDefault(e => e.Id == id);
            return game;
        }

        public bool IsUserAuthorizedToSeeGameDetails(string userId, Game game)
        {
            IEnumerable<User> players = game.Group.Players;

            if (players.Select(p => p.Id).Contains(userId))
            {
                return true;
            }

            return false;
        }

        public bool IsUserAuthorizedToModifyGame(string userId, Game game)
        {
            if (game.Group.Admins.Select(p => p.Id).Contains(userId))
            {
                return true;
            }

            return false;
        }

        public void JoinGame(string id, string userId)
        {
            Game game = this.Context.Games.Find(id);
            User user = this.Context.Users.Find(userId);
            UserGame ug = new UserGame()
            {
                User = user,
                Game = game
            };

            game.Players.Add(ug);

            this.Context.SaveChanges();
        }

        public Game GetNextGame(string userId)
        {
            IEnumerable<Group> groups = this.Context.Users.Include(u => u.GroupsPlaying.Select(g => g.Games))
                .SingleOrDefault(u => u.Id == userId)?.GroupsPlaying.ToList();
            IList<Game> games = new List<Game>();
            foreach (var group in groups)
            {
                foreach (var g in group.Games)
                {
                    if (g.Start > DateTime.Now)
                    {
                        games.Add(g);
                    }
                }
            }

            Game game = games.OrderBy(g => g.Start).FirstOrDefault();

            return game;
        }

        public void LeaveGame(string gameId, string userId)
        {
            Game game = this.Context.Games.Find(gameId);
            UserGame player = this.Context.UserGames.Find(userId, gameId);
            game?.Players.Remove(player);

            this.Context.SaveChanges();
        }

        public void CancelGame(string gameId, string userId)
        {
            Game game = this.Context.Games.Find(gameId);
            if (game != null)
            {
                game.IsCanceled = true;

                User requester = this.Context.Users.Find(userId);
                foreach (var player in game.Group.Players)
                {
                    CanceledGameNotification notification = new CanceledGameNotification()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Game = game,
                        Requester = requester
                    };

                    player.Notifications.Add(notification);
                }

                Context.SaveChanges();
            }
        }
    }
}