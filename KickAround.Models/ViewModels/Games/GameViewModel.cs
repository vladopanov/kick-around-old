using System;
using System.ComponentModel;

namespace KickAround.Models.ViewModels.Games
{
    public class GameViewModel
    {
        public string Id { get; set; }

        [DisplayName("Group Name")]
        public string GroupName { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        [DisplayName("Is Weekly")]
        public bool IsWeekly { get; set; }

        [DisplayName("Is Canceled")]
        public bool IsCanceled { get; set; }
    }
}
