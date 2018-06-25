using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using KickAround.Models.EntityModels.Notifications;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KickAround.Models.EntityModels
{
    public class User : IdentityUser
    {
        public User()
        {
            this.GroupsPlaying = new HashSet<Group>();
            this.GroupsAdmin = new HashSet<Group>();
            this.Games = new HashSet<UserGame>();
            this.Notifications = new HashSet<Notification>();
            this.UserImages = new HashSet<UserImage>();
        }

        // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
        public virtual ICollection<Group> GroupsPlaying { get; set; }

        public virtual ICollection<Group> GroupsAdmin { get; set; }

        public virtual ICollection<UserGame> Games { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        public virtual ICollection<UserImage> UserImages { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
