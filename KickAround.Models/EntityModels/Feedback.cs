using System;
using System.ComponentModel.DataAnnotations;

namespace KickAround.Models.EntityModels
{
    public class Feedback
    {
        public Feedback()
        {
            this.CreatedOn = DateTime.Now;
        }

        public string Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(30)]
        public string Subject { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual User Sender { get; set; }
    }
}
