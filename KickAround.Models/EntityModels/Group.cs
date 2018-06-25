using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KickAround.Models.Enums;

namespace KickAround.Models.EntityModels
{
    public class Group
    {
        public Group()
        {
            this.Games = new HashSet<Game>();
            this.Players = new HashSet<User>();
            this.Admins = new HashSet<User>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public Sports Sport { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        [InverseProperty("GroupsPlaying")]
        public virtual ICollection<User> Players { get; set; }

        [InverseProperty("GroupsAdmin")]
        public virtual ICollection<User> Admins { get; set; }
    }
}
