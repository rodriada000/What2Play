using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameDecider.Models;

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

        public ActionResult AddGame(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            using (WebClient wc = new WebClient())
            {
                string url = "https://www.igdb.com/api/v1/games/" + id + "?token=RdX2gpnNPeXJktPPCmnKt4E4BG5FoJXsUh5-gFARXOY";
                var json = wc.DownloadString(url);
                if (json != null)
                {
                    Dictionary<string, object> games = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    return View(games["game"]);
                }
            }

            return PartialView(db.Platforms.ToList());
        }
    }

    public class GameDetails
    {

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