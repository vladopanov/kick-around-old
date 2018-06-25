using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KickAround.Models.Enums;

namespace KickAround.Models.BindingModels.Groups
{
    public class CreateGroupBindingModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public Sports Sport { get; set; }

        [MinLength(2)]
        [MaxLength(50)]
        public string Country { get; set; }

        [MinLength(2)]
        [MaxLength(50)]
        public string Town { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string Address { get; set; }
    }
}
