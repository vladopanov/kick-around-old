using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KickAround.Models.EntityModels
{
    public class Game
    {
        public Game()
        {
            this.Players = new HashSet<UserGame>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public DateTime? Start { get; set; }

        [Required]
        public DateTime? End { get; set; }

        public bool IsWeekly { get; set; }

        public bool IsCanceled { get; set; }

        public bool IsPattern { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<UserGame> Players { get; set; }
    }
}
