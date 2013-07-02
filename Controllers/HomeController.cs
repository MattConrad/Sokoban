using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sokoban.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["CurrentGame"] == null)
                Session["CurrentGame"] = new Models.Game();

            Models.Game gameCurrent = (Models.Game)Session["CurrentGame"];

            ViewData["GameHTML"] = gameCurrent.GetHTMLDisplay();
            ViewData["Moves"] = gameCurrent.Moves;
            ViewData["FinishedInMoves"] = gameCurrent.FinishedInMoves;
            
            return View();
        }

        [HttpPost]
        public ActionResult NextLevel()
        {
            Models.Game g = (Models.Game)Session["CurrentGame"];

            g = g.NextLevel();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AjaxMoveHandler(string GameAction)
        {
            Models.Game g = (Models.Game)Session["CurrentGame"];

            if (g.SupportedActions.Contains(GameAction))
                g = g.ProcessGameAction(GameAction);

            ViewData["GameHTML"] = g.GetHTMLDisplay();
            ViewData["Moves"] = g.Moves;
            ViewData["FinishedInMoves"] = g.FinishedInMoves;

            return PartialView("Gameboard");
        }

        public ActionResult About()
        {
            return View();
        }

    }
}
