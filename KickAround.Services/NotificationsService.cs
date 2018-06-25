using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using KickAround.Models.EntityModels;
using KickAround.Models.EntityModels.Notifications;
using KickAround.Models.ViewModels.Notifications;
using KickAround.Services.Contracts;

namespace KickAround.Services
{
    public class NotificationsService : Service, INotificationsService
    {
        public int GetNumberOfUnreadNotifications(string userId)
        {
            var user = this.Context.Users.Include(u => u.Notifications)
                .SingleOrDefault(u => u.Id == userId);

            int count = 0;
            if (user != null)
            {
                count = user.Notifications.Count(n => !n.IsRead);
            }

            return count;
        }

        public IEnumerable<NotificationViewModel> GetUserNotifications(string userId)
        {
            IEnumerable<NotificationViewModel> viewModel = this.Context.Users.Include(u => u.Notifications)
                .SingleOrDefault(u => u.Id == userId)?.Notifications
                .OrderByDescending(n => n.CreatedOn)
                .Select(n => new NotificationViewModel()
                {
                    Id = n.Id,
                    IsRead = n.IsRead,
                    Date = n.CreatedOn.Value,
                    Content = n.ToString()
                });

            return viewModel;
        }

        public void AcceptJoinGroupRequest(string id, string userId)
        {
            JoinGroupRequestNotification notification = this.Context.Notifications.OfType<JoinGroupRequestNotification>()
                .Include(n => n.Requester)
                .Include(n => n.Group)
                .SingleOrDefault(n => n.Id == id);

            Group group = notification?.Group;
            User requester = notification?.Requester;
            User sender = this.Context.Users.Find(userId);

            if (group?.Players != null)
            {
                foreach (var player in group.Players)
                {
                    PlayerJoinedGroupNotification playerJoinedGroupNotification = new PlayerJoinedGroupNotification()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Group = group,
                        Requester = requester
                    };

                    player.Notifications.Add(playerJoinedGroupNotification);
                }

                this.Context.Notifications.Remove(notification);
                group.Players.Add(requester);
            }

            PlayerAcceptedGroupNotification playerAcceptedGroupNotification = new PlayerAcceptedGroupNotification()
            {
                Id = Guid.NewGuid().ToString(),
                Group = group,
                Sender = sender
            };
            requester?.Notifications.Add(playerAcceptedGroupNotification);

            this.Context.SaveChanges();
        }

        public void RejectJoinGroupRequest(string id, string userId)
        {
            JoinGroupRequestNotification notification = this.Context.Notifications.OfType<JoinGroupRequestNotification>()
                .Include(n => n.Requester)
                .Include(n => n.Group)
                .SingleOrDefault(n => n.Id == id);

            Group group = notification?.Group;
            User requester = notification?.Requester;
            User sender = this.Context.Users.Find(userId);

            if (group?.Players != null)
            {
                PlayerRejectedGroupNotification playerRejectedGroupNotification = new PlayerRejectedGroupNotification()
                {
                    Id = Guid.NewGuid().ToString(),
                    Group = group,
                    Sender = sender
                };

                requester.Notifications.Add(playerRejectedGroupNotification);

                this.Context.Notifications.Remove(notification);
            }
            this.Context.SaveChanges();
        }

        public void ReadNotification(string id)
        {
            Notification notification = this.Context.Notifications.Find(id);
            if (notification != null) notification.IsRead = !notification.IsRead;

            this.Context.SaveChanges();
        }

        public void MarkAllNotificationAsRead(string getUserId)
        {
            IEnumerable<Notification> notifications = this.Context.Users
                .Include(u => u.Notifications)
                .SingleOrDefault(u => u.Id == getUserId)?.Notifications
                .Where(n => !(n is JoinGroupRequestNotification));

            if (notifications == null) return;
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            this.Context.SaveChanges();
        }
    }
}
