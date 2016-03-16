using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GameDecider.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(string gamesearch)
        {
            if (string.IsNullOrWhiteSpace(gamesearch))
            {
                return View(new List<IgdbGame>());
            }

            using (WebClient wc = new WebClient())
            {
                string url = "https://www.igdb.com/api/v1/games/search?q=" + gamesearch + "&token=RdX2gpnNPeXJktPPCmnKt4E4BG5FoJXsUh5-gFARXOY";
                var json = wc.DownloadString(url);
                if (json != null)
                {
                    Dictionary<string, List<IgdbGame>> games = JsonConvert.DeserializeObject<Dictionary<string, List<IgdbGame>>>(json);
                    return View(games["games"]);
                }
            }

            return View(new List<IgdbGame>()); // json error occured if made it here
        }
    }

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