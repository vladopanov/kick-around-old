using System.Text;

namespace KickAround.Models.EntityModels.Notifications
{
    public class JoinGroupRequestNotification : Notification
    {
        public virtual User Requester { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"<div>User <strong>{this.Requester.UserName}</strong> wants to join group <strong>{this.Group.Name}</strong>.</div>");
            sb.AppendLine($"<div><a href=\"/notifications/accept/{this.Id}\" class=\"btn btn-success btn-xs\" type=\"button\">Accept</a>");
            sb.AppendLine($"<a href=\"/notifications/reject/{this.Id}\" class=\"btn btn-danger btn-xs\" type=\"button\">Reject</a></div>");
            return sb.ToString();
        }
    }
}
