using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KickAround.Models.EntityModels
{
    public class UserGame
    {
        public UserGame()
        {
            this.CreatedOn = DateTime.Now;
        }

        [Key, Column(Order = 0), ForeignKey("User")]
        public string UserId { get; set; }
        [Key, Column(Order = 1), ForeignKey("Game")]
        public string GameId { get; set; }

        public User User { get; set; }
        public Game Game { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
