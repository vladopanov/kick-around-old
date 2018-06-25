using System;
using System.Collections.Generic;
using System.ComponentModel;
using KickAround.Models.EntityModels;

namespace KickAround.Models.ViewModels.Games
{
    public class GameDetailsViewModel
    {
        public string Id { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        [DisplayName("Is Weekly")]
        public bool IsWeekly { get; set; }

        [DisplayName("Is Canceled")]
        public bool IsCanceled { get; set; }

        public virtual Group Group { get; set; }

        public ICollection<UserGame> Players { get; set; }

        public bool IsJoined { get; set; }

        public bool IsAdmin { get; set; }
    }
}
