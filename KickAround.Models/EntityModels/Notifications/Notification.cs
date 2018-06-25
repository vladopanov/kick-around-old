using System;
using System.Collections.Generic;

namespace KickAround.Models.EntityModels.Notifications
{
    public abstract class Notification
    {
        protected Notification()
        {
            this.Receivers = new HashSet<User>();
            this.CreatedOn = DateTime.Now;
            this.IsRead = false;
        }

        public string Id { get; set; }

        public DateTime? CreatedOn { get; private set; }

        public bool IsRead { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<User> Receivers { get; set; }
    }
}
