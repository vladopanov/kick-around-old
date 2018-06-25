using System.Text;

namespace KickAround.Models.EntityModels.Notifications
{
    public class CanceledGameNotification : Notification
    {
        public virtual User Requester { get; set; }

        public virtual Game Game { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div");
            if (this.IsRead)
            {
                sb.AppendLine(" class='read'");
            }
            sb.AppendLine($">The game on <strong>{this.Game.Start}</strong> from group <strong>{this.Game.Group.Name}</strong> has been canceled by <strong>{this.Requester.UserName}</strong>.</div>");
            sb.Append(
                $"<div><a href=\"/notifications/read/{this.Id}\" class=\"btn btn-default btn-xs\" type=\"button\">");
            sb.Append(this.IsRead ? "Mark as Unread" : "Mark as Read");
            sb.Append("</a>");

            return sb.ToString();
        }
    }
}
