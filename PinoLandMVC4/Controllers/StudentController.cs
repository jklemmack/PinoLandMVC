using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PinoLandMVC4;
using PinoLandMVC4.Models;
using MG = Fuqua.CompetativeAnalysis.MarketGame;

namespace PinoLandMVC4.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/
        [Authorize()]
        public ActionResult Index()
        {
            using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
            {
                //Get user

                var games = from e in context.Economies
                            join c in context.Companies.Where(c => c.Users.Any(u => u.UserId == 101001) != false)
                            on e.EconomyId equals c.EconomyId
                            select new StudentGame()
                            {
                                EconomyId = e.EconomyId,
                                GameName = e.Name,
                                GameReference = e.Reference,
                                TeamId = c.CompanyId,
                                TeamName = c.Name
                            };

                int count = games.Count();
                if (count == 0)
                {
                    // no valid games
                    return View("NoValidGames");
                }
                else if (count == 1)    //only one - redirect to it
                {
                    var game = games.First();
                    return RedirectToAction("Details", new { economy = game.EconomyId, team = game.TeamId });
                }
                else // more than one, show them the list
                {
                    return View("GameList", games.ToList());
                }

            }
        }

        public ActionResult Details(int economy, int team)
        {
            StudentModel sm = new StudentModel();
            sm.News = new List<NewsModel>();

            using (MG.GameDataObjectContext context = ContextHelper.GetObjectContext())
            {
                List<FoodTeamAction> actions = context.GetCurrentActions(economy).Select(r => new FoodTeamAction()
                {
                    CapacityNow = r.CapacityLast ?? 0,
                    CapacityChange = (r.CompanyId == team) ? r.CapacityChange : 0,
                    EconomyId = r.EconomyId,
                    IsMine = r.CompanyId == team,
                    IsNew = !r.IsRollover,
                    Latitude = r.Latitude,
                    Longitude = r.Longitude,
                    Price = r.PriceLast ?? 0,
                    PriceNext = (r.CompanyId == team) ? r.Price : 0,
                    ProductionNext = (r.CompanyId == team) ? r.Production : 0,
                    RoundId = r.RoundId,
                    Team = r.CompanyName,
                    TeamId = r.CompanyId,
                    Type = r.FoodGoodType,
                    TypeId = r.TypeId

                }).ToList();
                sm.FoodActions = actions;

                // If the team has a new restaurant, note that for the View, since that should prevent them from adding a second
                ViewBag.HasNew = actions.Any(a => a.IsNew == true);
            }
            ViewBag.EconomyId = economy;
            return View(sm);
        }


        [NonAction]
        private int GetUserId(MG.GameDataObjectContext context)
        {
            MG.IM_User user = context.IM_User.FirstOrDefault(u => string.Compare(u.UserName, this.User.Identity.Name, false) == 0);
            if (user != null)
                return user.UserId;

            throw new ArgumentException("not a valid user");

        }

    }
}
