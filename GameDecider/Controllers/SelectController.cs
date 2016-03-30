using GameDecider.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameDecider.Controllers
{
    public class SelectController : Controller
    {
        // GET: Select
        public ActionResult Index()
        {
            var myGames = new List<UserVideoGame>();

            if (Request.IsAuthenticated) // retrieve list of games if logged in
            {
                var userId = User.Identity.GetUserId();
                ApplicationDbContext db = new ApplicationDbContext();
                myGames = db.UsersVideoGames.Where(g => g.UserID == userId).ToList();

                // get available platforms
                var plats = new List<Platform>();
                foreach (var g in myGames)
                {
                    if (plats.Contains(g.PlatformName) == false) // avoid duplicate platforms in list
                    {
                        plats.Add(g.PlatformName);
                    }
                }
                ViewBag.AvailPlatforms = plats;
            }

            return View(myGames);
        }

        //
        // GET: /Select/PickGame
        [HttpGet]
        public ActionResult PickGame(string platformvalue, string favOnly)
        {
            string chosenGame = "GAME";
            ApplicationDbContext db = new ApplicationDbContext();
            var myGames = new List<UserVideoGame>();

            var userId = User.Identity.GetUserId();

            if (platformvalue == "all")
            {
                if (favOnly == "true")
                {
                    // get list of all user's games that are favorites
                    myGames = db.UsersVideoGames.Where(g => g.UserID == userId && g.Favorite == true).ToList();
                }
                else
                {
                    // get list of all user's games 
                    myGames = db.UsersVideoGames.Where(g => g.UserID == userId).ToList();
                }
            }
            else
            {
                var platId = int.Parse(platformvalue);
                if (favOnly == "true")
                {
                    // get list of user's games that are favorites for specific platform
                    myGames = db.UsersVideoGames.Where(g => g.UserID == userId && g.PlatformID == platId && g.Favorite == true).ToList();
                }
                else
                {
                    // get list of all user's games  for specific platform
                    myGames = db.UsersVideoGames.Where(g => g.UserID == userId && g.PlatformID == platId).ToList(); 
                }
            }



            Random rand = new Random();
            int index = rand.Next(myGames.Count()); // choose a random 
            chosenGame = myGames[index].GameID.GameName;
            if (Session["PreviousChoice"] != null)
            {
                string prev = Session["PreviousChoice"] as string;
                while (chosenGame == prev && myGames.Count > 1) // prevent the same game from being chosen
                {
                    index = rand.Next(myGames.Count()); // choose a random index
                    chosenGame = myGames[index].GameID.GameName;
                }
                Session["PreviousChoice"] = chosenGame;
            }
            else
            {
                Session.Add("PreviousChoice", chosenGame);
            }

            ViewBag.Game = chosenGame;
            return PartialView("SelectRandomGame");
        }
    }
}