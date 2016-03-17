using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameDecider.Models
{
    public class VideoGame
    {
        public int VideoGameID { get; set; }
        public int GameID { get; set; }
        public virtual Platform PlatformName { get; set; }
        public bool Favorite { get; set; }
    }
}