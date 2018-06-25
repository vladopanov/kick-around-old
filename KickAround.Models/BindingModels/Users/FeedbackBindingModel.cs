using System.ComponentModel.DataAnnotations;

namespace KickAround.Models.BindingModels.Users
{
    public class FeedbackBindingModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(30)]
        public string Subject { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}
