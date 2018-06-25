using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KickAround.Models.EntityModels;
using KickAround.Models.Enums;

namespace KickAround.Models.ViewModels.Groups
{
    public class GroupDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Sports Sport { get; set; }

        [Display(Name = "Total of Players")]
        public int NumberOfPlayers { get; set; }

        public string Address { get; set; }

        public IList<User> Players { get; set; }

        public IList<User> Admins { get; set; }

        public bool IsJoined { get; set; }

        public bool HasRequested { get; set; }

        public bool IsAdmin { get; set; }
    }
}
