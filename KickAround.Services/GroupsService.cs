using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using KickAround.Models.BindingModels.Groups;
using KickAround.Models.EntityModels;
using KickAround.Models.EntityModels.Notifications;
using KickAround.Models.ViewModels.Groups;
using KickAround.Services.Contracts;

namespace KickAround.Services
{
    public class GroupsService : Service, IGroupsService
    {
        public IEnumerable<GroupViewModel> GetAllGroups(string userId)
        {
            var user = Context.Users
                .Include(u => u.GroupsPlaying.Select(g => g.Players))
                .SingleOrDefault(u => u.Id == userId);

            IEnumerable<GroupViewModel> allGroups = user?.GroupsPlaying
                .Select(group => new GroupViewModel
                {
                    Id = group.Id,
                    Name = group.Name,
                    Sport = group.Sport,
                    NumberOfPlayers = group.Players.Count
                });

            return allGroups?.ToList();
        }

        public object GetGroupDetails(string groupdId, string userId)
        {
            Group group = GetGroup(groupdId);
            User user = Context.Users.Find(userId);
            string[] locationArr = {group.Location.Country, group.Location.Town, group.Location.Address};

            GroupDetailsViewModel groupDetails = new GroupDetailsViewModel
            {
                Id = group.Id,
                Name = group.Name,
                Sport = group.Sport,
                NumberOfPlayers = group.Players.Count,
                Address = string.Join(", ", locationArr),
                Players = group.Players.ToList(),
                Admins = group.Admins.ToList(),
                IsJoined = group.Players.Select(p => p.Id).Contains(userId),
                HasRequested = user.Notifications.OfType<JoinGroupRequestNotification>().Any(n => n.Group == group),
                IsAdmin = group.Admins.Select(a => a.Id).Contains(userId)
            };

            return groupDetails;
        }

        public void CreateGroup(CreateGroupBindingModel bindingModel, string userId)
        {
            var group = Mapper.Map<CreateGroupBindingModel, Group>(bindingModel);
            group.Id = Guid.NewGuid().ToString();
            var location = Mapper.Map<CreateGroupBindingModel, Location>(bindingModel);
            location.Id = Guid.NewGuid().ToString();

            var player = Context.Users.Find(userId);
            if (player != null)
            {
                group.Admins.Add(player);
                group.Players.Add(player);
            }

            group.Location = location;
            Context.Groups.Add(group);
            Context.SaveChanges();
        }

        public object GetEditGroupModel(string id)
        {
            Group group = GetGroup(id);

            var model = Mapper.Map<Group, EditGroupBindingModel>(group);
            model.Country = group.Location.Country;
            model.Town = group.Location.Town;
            model.Address = group.Location.Address;

            return model;
        }

        public void EditGroup(EditGroupBindingModel bindingModel)
        {
            var group = Context.Groups.Find(bindingModel.Id);

            if (group != null)
            {
                group.Name = bindingModel.Name;
                group.Sport = bindingModel.Sport;
                group.Location.Country = bindingModel.Country;
                group.Location.Town = bindingModel.Town;
                group.Location.Address = bindingModel.Address;
            }

            Context.SaveChanges();
        }

        public void DeleteGroup(string id)
        {
            Group group = GetGroup(id);
            Location location = group.Location;
            IEnumerable<Game> games = group.Games;
            foreach (var g in games)
            {
                IEnumerable<UserGame> usergames = Context.UserGames.Where(ug => ug.GameId == g.Id);
                Context.UserGames.RemoveRange(usergames);

                IEnumerable<Notification> notifications = Context.Notifications
                    .Where(n => n.Group == group);
                Context.Notifications.RemoveRange(notifications);
            }

            Context.Locations.Remove(location);
            Context.Games.RemoveRange(games);
            Context.Groups.Remove(group);
            Context.SaveChanges();
        }

        public Group GetGroup(string id)
        {
            Group group = Context.Groups
                .Include(g => g.Players)
                .Include(g => g.Admins)
                .Include(g => g.Games)
                .SingleOrDefault(e => e.Id == id);

            return group;
        }

        public bool IsUserAuthorizedToSeeGroupDetails(string userId, Group group)
        {
            if (group.Players.Select(p => p.Id).Contains(userId))
            {
                return true;
            }

            return false;
        }

        public bool IsUserAuthorizedToModifyGroup(string userId, Group group)
        {
            if (group.Admins.Select(p => p.Id).Contains(userId))
            {
                return true;
            }

            return false;
        }

        public void JoinGroupRequest(string groupId, string userId)
        {
            Group group = Context.Groups.Find(groupId);
            User user = Context.Users.Find(userId);
            IEnumerable<User> admins = Context.Groups.Include(g => g.Admins)
                .SingleOrDefault(g => g.Id == groupId)?.Admins;

            if (admins != null)
            {
                JoinGroupRequestNotification notification = new JoinGroupRequestNotification
                {
                    Id = Guid.NewGuid().ToString(),
                    Group = group,
                    Requester = user
                };

                foreach (var admin in admins)
                {
                    admin.Notifications.Add(notification);
                }

                Context.SaveChanges();
            }
        }

        public IEnumerable<Group> SearchGroups(string query)
        {
            IEnumerable<Group> groups = Context.Groups.Where(g => g.Name.Contains(query))
                .OrderBy(g => g.Name).ToList();
            return groups;
        }
    }
}
