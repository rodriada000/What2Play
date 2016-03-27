using System;
using System.ComponentModel;

namespace GameDecider.Models
{
    /// <summary>
    /// This model is used for parsing json data from IGDB search
    /// Used in the SearchController/View
    /// </summary>
    public class IgdbGame
    {
        public int id { get; set; }

        [DisplayName("Name")]
        public string name { get; set; }

        public string slug { get; set; }

        [DisplayName("Release Date")]
        public DateTime? release_date { get; set; }

        public string cover { get; set; }
        public string cover_id { get; set; }
    }
}