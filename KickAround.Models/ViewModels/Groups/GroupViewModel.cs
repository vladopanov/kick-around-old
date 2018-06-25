using System.ComponentModel.DataAnnotations;
using KickAround.Models.Enums;

namespace KickAround.Models.ViewModels.Groups
{
    public class GroupViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Sports Sport { get; set; }

        [Display(Name = "Total of Players")]
        public int NumberOfPlayers { get; set; }
    }
}
