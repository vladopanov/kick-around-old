using System;
using System.ComponentModel.DataAnnotations;

namespace KickAround.Models.BindingModels.Games
{
    public class EditGameBindingModel
    {
        public string Id { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public bool IsWeekly { get; set; }
    }
}
