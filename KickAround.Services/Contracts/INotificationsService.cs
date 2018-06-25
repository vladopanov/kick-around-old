using System.Collections.Generic;
using KickAround.Data;
using KickAround.Models.ViewModels.Notifications;

namespace KickAround.Services.Contracts
{
    public interface INotificationsService
    {
        int GetNumberOfUnreadNotifications(string userId);
        IEnumerable<NotificationViewModel> GetUserNotifications(string userId);
        void AcceptJoinGroupRequest(string id, string userId);
        void RejectJoinGroupRequest(string id, string userId);
        void ReadNotification(string id);
        void MarkAllNotificationAsRead(string getUserId);
        KickAroundContext Context { get; }
    }
}