using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KickAround.Models.BindingModels.Games
{
    public class CreateGameBindingModel
    {
        [Required]
        public DateTime? Start { get; set; }

        [Required]
        public DateTime? End { get; set; }

        [DisplayName("Is Weekly")]
        public bool IsWeekly { get; set; }
    }
}
