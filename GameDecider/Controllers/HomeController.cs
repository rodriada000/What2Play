using GameDecider.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GameDecider.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SelectRandomGame()
        {
            string chosenGame = "GAME";
            ApplicationDbContext db = new ApplicationDbContext();
            var userId = User.Identity.GetUserId();
            var userGames = db.VideoGames.Where(g => g.UserID == userId).ToList(); // get list of user's games
            Random rand = new Random();
            int index = rand.Next(userGames.Count()); // choose a random index

            if (Session["MyGameNames"] == null)
            {
                int gameID = userGames[index].GameID; // access a random game id
                using (WebClient wc = new WebClient())
                {
                    string token = System.Configuration.ConfigurationManager.AppSettings["IGDB_API_KEY"];
                    string url = "https://www.igdb.com/api/v1/games/" + gameID.ToString() + "?token=" + token;
                    var json = wc.DownloadString(url);
                    if (json != null)
                    {
                        RootObject game = JsonConvert.DeserializeObject<RootObject>(json);
                        chosenGame = game.game.name;
                    }
                }
            }
            else // Used cached names to pick random game
            {
                var gameNames = Session["MyGameNames"] as List<string>;
                chosenGame = gameNames[index];
            }

            ViewBag.Game = chosenGame;
            return PartialView();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}