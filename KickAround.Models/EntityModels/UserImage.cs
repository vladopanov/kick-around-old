namespace KickAround.Models.EntityModels
{
    public class UserImage
    {
        public string Id { get; set; }

        public string ImageUrl { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
