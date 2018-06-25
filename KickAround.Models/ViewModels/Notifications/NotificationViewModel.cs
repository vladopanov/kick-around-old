using System;
using System.ComponentModel;

namespace KickAround.Models.ViewModels.Notifications
{
    public class NotificationViewModel
    {
        public string Id { get; set; }

        public bool IsRead { get; set; }

        [DisplayName("Published on")]
        public DateTime Date { get; set; }

        public string Content { get; set; }
    }
}
