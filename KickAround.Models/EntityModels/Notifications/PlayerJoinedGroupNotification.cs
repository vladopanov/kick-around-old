using System.Text;

namespace KickAround.Models.EntityModels.Notifications
{
    public class PlayerJoinedGroupNotification : Notification
    {
        public virtual User Requester { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div");
            if (this.IsRead)
            {
                sb.AppendLine(" class='read'");
            }
            sb.AppendLine($">User <strong>{this.Requester.UserName}</strong> has joined group <strong>{this.Group.Name}</strong>.</div>");
            sb.Append(
                $"<div><a href=\"/notifications/read/{this.Id}\" class=\"btn btn-default btn-xs\" type=\"button\">");
            sb.Append(this.IsRead ? "Mark as Unread" : "Mark as Read");
            sb.Append("</a>");

            return sb.ToString();
        }
    }
}
