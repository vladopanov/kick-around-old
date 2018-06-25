using System.ComponentModel.DataAnnotations;

namespace KickAround.Models.EntityModels
{
    public class Location
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Country { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Town { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string Address { get; set; }

        public virtual Group Group { get; set; }
    }
}
