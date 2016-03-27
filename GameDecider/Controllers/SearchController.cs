using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameDecider.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Web.Caching;

namespace GameDecider.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Search
        public ActionResult Index(string gamesearch)
        {
            if (string.IsNullOrWhiteSpace(gamesearch))
            {
                return View(new List<IgdbGame>());
            }

            using (WebClient wc = new WebClient())
            {
                string token = System.Configuration.ConfigurationManager.AppSettings["IGDB_API_KEY"];
                string url = "https://www.igdb.com/api/v1/games/search?q=" + gamesearch + "&token=" + token;
                var json = wc.DownloadString(url);
                if (json != null)
                {
                    Dictionary<string, List<IgdbGame>> games = JsonConvert.DeserializeObject<Dictionary<string, List<IgdbGame>>>(json);
                    return View(games["games"]);
                }
            }

            return View(new List<IgdbGame>()); // json error occured if made it here
        }

        [HttpGet]
        public ActionResult AddGame(string id_str)
        {
            //List<string> available_plats = new List<string>();
            //if (id_str != null)
            //{
            //    using (WebClient wc = new WebClient())
            //    {
            //        string token = System.Configuration.ConfigurationManager.AppSettings["IGDB_API_KEY"];
            //        string url = "https://www.igdb.com/api/v1/games/" + id_str + "?token=" + token;
            //        var json = wc.DownloadString(url);
            //        if (json != null)
            //        {
            //            RootObject game = JsonConvert.DeserializeObject<RootObject>(json);
            //            if (game.game.release_dates != null)
            //            {
            //                foreach (ReleaseDate r in game.game.release_dates)
            //                {
            //                    if (r.platform_name == "Microsoft Windows" || r.platform_name == "Mac" || r.platform_name == "Linux")
            //                    {
            //                        available_plats.Add("PC"); // Unify any computer game to be under PC
            //                    }
            //                    else
            //                    {
            //                        available_plats.Add(r.platform_name);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            ViewBag.GameId = id_str;
            return PartialView(db.Platforms.ToList());
        }

        [HttpPost]
        public ActionResult AddGame(string plat_id_str, string game_id)
        {
            VideoGame game = new VideoGame();
            game.GameID = int.Parse(game_id);
            game.Favorite = false;
            int plat_id = int.Parse(plat_id_str);
            game.PlatformName = db.Platforms.AsEnumerable().ElementAt(plat_id - 1);

            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                game.UserID = userId;
                db.VideoGames.Add(game);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }

}