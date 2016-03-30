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
        public ActionResult Index(string gamesearch, string status)
        {
            if (status != null) // user has added a game
            {
                ViewBag.Status = status;
                return View(new List<IgdbGame>());
            }

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
            Dictionary<string, Platform> platforms = db.Platforms.ToDictionary(k => k.Name);
            List<Platform> available_plats = new List<Platform>();
            if (id_str != null)
            {
                using (WebClient wc = new WebClient())
                {
                    string token = System.Configuration.ConfigurationManager.AppSettings["IGDB_API_KEY"];
                    string url = "https://www.igdb.com/api/v1/games/" + id_str + "?token=" + token;
                    var json = wc.DownloadString(url);
                    if (json != null)
                    {
                        RootObject game = JsonConvert.DeserializeObject<RootObject>(json);
                        if (game.game.release_dates != null)
                        {
                            foreach (ReleaseDate r in game.game.release_dates)
                            {
                                if (r.platform_name == "Microsoft Windows" || r.platform_name == "Mac" || r.platform_name == "Linux")
                                {
                                    if (available_plats.Contains(platforms["PC"]) == false) // prevent adding multiple "PC" platforms
                                    {
                                        available_plats.Add(platforms["PC"]);
                                    }
                                }
                                else
                                {
                                    try {
                                        available_plats.Add(platforms[r.platform_name]);
                                    }
                                    catch (Exception e) {
                                        // hmmmmm missing key
                                    }
                                }
                            }
                        }
                    }
                }
            }
            int game_id = int.Parse(id_str);
            ViewBag.GameId = game_id;
            ViewBag.AllPlats = platforms.Values.ToList();
            return PartialView(available_plats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddGame(string plat_id_str, int game_id)
        {
            int plat_id = int.Parse(plat_id_str);

            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                // Add name/id to VideoGames table if doesn't exist
                if (GameExists(game_id) == false)
                {
                    VideoGame igdbGame = new VideoGame();
                    igdbGame.IgdbID = game_id;
                    igdbGame.GameName = GetName(game_id);
                    if (igdbGame.GameName != null)
                    {
                        db.VideoGameNames.Add(igdbGame);
                        db.SaveChanges();
                    }
                }

                UserVideoGame game = new UserVideoGame();
                game.UserID = userId;
                game.IgdbID = game_id;
                game.Favorite = false;
                game.PlatformID = plat_id;

                // Verify game is not already added
                var usersGames = db.UsersVideoGames.Where(g => g.IgdbID == game_id && g.UserID == userId && g.PlatformID == plat_id).ToList();
                if (usersGames.Count > 0)
                {
                    return RedirectToAction("Index", new { status = "Duplicate" });
                }

                db.UsersVideoGames.Add(game);
                db.SaveChanges();
                Session.Remove("MyGames"); // remove list from session so new game can be added to the Session
                return RedirectToAction("Index", new { status = "Added" });
            }
            else
            {
                return RedirectToAction("Index", new { status = "Failed" });
            }
        }

        // Check if the game name is already stored in the DB
        private bool GameExists(int id)
        {
            var game = db.VideoGameNames.Where(g => g.IgdbID == id).ToList();

            if (game == null || game.Count == 0)
            {
                return false;
            }
            return true;
        }

        // Fetch game name from IGDB API
        private string GetName(int id)
        {
            using (WebClient wc = new WebClient())
            {
                string token = System.Configuration.ConfigurationManager.AppSettings["IGDB_API_KEY"];
                string url = "https://www.igdb.com/api/v1/games/" + id.ToString() + "?token=" + token;
                var json = wc.DownloadString(url);
                if (json != null)
                {
                    RootObject game = JsonConvert.DeserializeObject<RootObject>(json);
                    return game.game.name;
                }
                else
                {
                    return null;
                }
            }
        }
    }

}