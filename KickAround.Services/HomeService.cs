using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using KickAround.Models.BindingModels.Users;
using KickAround.Models.EntityModels;
using KickAround.Services.Contracts;

namespace KickAround.Services
{
    public class HomeService : Service, IHomeService
    {
        public void PopulateGames(string userId)
        {
            User user = this.Context.Users
                .Include(u => u.GroupsPlaying.Select(g => g.Games))
                .SingleOrDefault(u => u.Id == userId);

            if (user != null && user.GroupsPlaying.Any())
                foreach (var group in user.GroupsPlaying)
                {
                    if (!group.Games.Any(g => g.Start != null && (g.IsWeekly && g.Start.Value > DateTime.Today)))
                    {
                        IEnumerable<Game> patternGames = group.Games.Where(g => g.IsPattern).ToList();
                        foreach (var game in patternGames)
                        {
                            PopulateWeeklyGames(game, group);
                        }
                    }
                }
        }

        private void PopulateWeeklyGames(Game game, Group group)
        {
            DateTime dt = DateTime.Today;
            int startDays = dt.DayOfWeek - game.Start.Value.DayOfWeek;
            int endDays = dt.DayOfWeek - game.End.Value.DayOfWeek;
            if (startDays < 1)
            {
                startDays = Math.Abs(startDays);
            }
            else
            {
                startDays = -startDays;
            }

            if (endDays < 1)
            {
                endDays = Math.Abs(endDays);
            }
            else
            {
                endDays = -endDays;
            }

            var months = DateTime.Now.Month + 1;
            var startDate = dt.AddDays(startDays);
            var endDate = dt.AddDays(endDays);
            startDate = startDate.AddHours(game.Start.Value.Hour);
            startDate = startDate.AddMinutes(game.Start.Value.Minute);
            endDate = endDate.AddHours(game.End.Value.Hour);
            endDate = endDate.AddMinutes(game.End.Value.Minute);

            List<Game> games = new List<Game>();

            while (startDate.Month <= months)
            {
                Game currentGame = new Game()
                {
                    Id = Guid.NewGuid().ToString(),
                    IsWeekly = true,
                    Start = startDate,
                    End = endDate,
                    Group = group,
                    IsCanceled = false
                };
                games.Add(currentGame);

                startDate = startDate.AddDays(7);
                endDate = endDate.AddDays(7);
            }

            this.Context.Games.AddRange(games);
            this.Context.SaveChanges();
        }

        public bool HasCurrentUserGroups(string userId)
        {
            var singleOrDefault = this.Context.Users.Include(g => g.GroupsPlaying)
                .SingleOrDefault(u => u.Id == userId);
            bool hasGroups = singleOrDefault != null && singleOrDefault.GroupsPlaying.Any();

            return hasGroups;
        }

        public void CreateFeedback(FeedbackBindingModel bindingModel, string userId)
        {
            Feedback feedback = Mapper.Map<FeedbackBindingModel, Feedback>(bindingModel);
            feedback.Id = Guid.NewGuid().ToString();
            User sender = this.Context.Users.Find(userId);
            feedback.Sender = sender;

            this.Context.Feedbacks.Add(feedback);

            this.Context.SaveChanges();
        }
    }
}