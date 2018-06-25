using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using KickAround.Models.BindingModels.Games;
using KickAround.Models.EntityModels;
using KickAround.Models.ViewModels.Games;
using KickAround.Services.Contracts;
using Microsoft.AspNet.Identity;

namespace KickAround.Web.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private readonly IGamesService _service;

        public GamesController(IGamesService service)
        {
            this._service = service;
        }

        // GET: Games
        //public ActionResult Index()
        //{
        //    IEnumerable<GameViewModel> games = this._service.GetAllGames(this.HttpContext.User.Identity.GetUserId());
        //    return View(games);
        //}

        // GET: Games/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Game game = this._service.GetGame(id);

            if (game == null)
            {
                return HttpNotFound();
            }

            if (!this._service.IsUserAuthorizedToSeeGameDetails(this.HttpContext.User.Identity.GetUserId(), game))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            GameDetailsViewModel gameDetails = this._service.GetGameDetails(id, this.HttpContext.User.Identity.GetUserId());

            return View(gameDetails);
        }

        // GET: Games/Create
        public ActionResult Create(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string id, [Bind(Include = "Start, End, IsWeekly")] CreateGameBindingModel bindingModel)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                this._service.CreateGame(id, bindingModel);
                
                return RedirectToAction("Calendar");
            }

            return View(bindingModel);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Game game = this._service.GetGame(id);

            if (game == null)
            {
                return HttpNotFound();
            }

            if (!this._service.IsUserAuthorizedToModifyGame(this.HttpContext.User.Identity.GetUserId(), game))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            EditGameBindingModel gameToEdit = this._service.GetEditGameModel(game);

            return View(gameToEdit);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Start,End,IsWeekly,IsCanceled")] EditGameBindingModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                this._service.EditGame(bindingModel);
                return RedirectToAction("Calendar");
            }
            return View(bindingModel);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Game game = this._service.GetGame(id);
            if (game == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (!this._service.IsUserAuthorizedToModifyGame(this.HttpContext.User.Identity.GetUserId(), game))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this._service.DeleteGame(id);

            return RedirectToAction("Calendar");
        }

        public ActionResult Calendar()
        {
            return View();
        }

        public ActionResult Join(string id)
        {
            this._service.JoinGame(id, this.HttpContext.User.Identity.GetUserId());

            return RedirectToAction("Details", new { id });
        }

        public JsonResult GetCalendarEvents()
        {
            IEnumerable<GameViewModel> games = this._service.GetAllGames(this.HttpContext.User.Identity.GetUserId());
            IEnumerable<CalendarViewModel> calendar = games.Select(g => new CalendarViewModel()
            {
                title = g.GroupName,
                start = g.Start.ToString(),
                end = g.End.ToString(),
                url = "/games/details/" + g.Id
            });

            return this.Json(calendar, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNextGame()
        {
            Game game = this._service.GetNextGame(this.HttpContext.User.Identity.GetUserId());

            if (game != null)
            {
                NextGameViewModel viewModel = new NextGameViewModel()
                {
                    id = game.Id,
                    start = game.Start.ToString(),
                    groupName = game.Group.Name
                };
                return this.Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            NextGameViewModel viewModelNull = new NextGameViewModel()
            {
                id = "",
                start = ""
            };
            return this.Json(viewModelNull, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._service.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Leave(string id)
        {
            string userId = this.HttpContext.User.Identity.GetUserId();
            this._service.LeaveGame(id, userId);

            return RedirectToAction("Details", new {id});
        }

        public ActionResult Cancel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Game game = this._service.GetGame(id);
            if (game == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (!this._service.IsUserAuthorizedToModifyGame(this.HttpContext.User.Identity.GetUserId(), game))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(string id)
        {
            this._service.CancelGame(id, this.HttpContext.User.Identity.GetUserId());

            return RedirectToAction("Calendar");
        }
    }
}
