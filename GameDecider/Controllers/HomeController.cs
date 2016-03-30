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
            var myGames = db.UsersVideoGames.Where(g => g.UserID == userId).ToList(); // get list of user's games
            Random rand = new Random();
            int index = rand.Next(myGames.Count()); // choose a random index

            chosenGame = myGames[index].GameID.GameName;
            if (Session["PreviousChoice"] != null)
            {
                string prev = Session["PreviousChoice"] as string;
                while (chosenGame == prev) // prevent the same game from being chosen
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