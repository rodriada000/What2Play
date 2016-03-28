using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GameDecider.Models
{
    public class UserVideoGame
    {
        public int UserVideoGameID { get; set; }
        public bool Favorite { get; set; }

        public int IgdbID { get; set; }
        public virtual VideoGame GameID { get; set; } // Foreign key for VideoGame.IgddbID

        public int PlatformID { get; set; }
        public virtual Platform PlatformName { get; set; }

        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class VideoGame
    {
        public VideoGame()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IgdbID { get; set; } // IGDB ID of a video game

        [MaxLength(512)]
        public string GameName { get; set; }
    }
}