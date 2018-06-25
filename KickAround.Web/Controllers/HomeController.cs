using System.Web.Mvc;
using KickAround.Models.BindingModels.Users;
using KickAround.Services.Contracts;
using Microsoft.AspNet.Identity;

namespace KickAround.Web.Controllers
{
    //[RequireHttps]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService _service;

        public HomeController(IHomeService service)
        {
            this._service = service;
        }

        //[AllowAnonymous]
        //public ActionResult Welcome()
        //{
        //    return View();
        //}

        public ActionResult Dashboard()
        {
            var hasGroups = this._service.HasCurrentUserGroups(this.HttpContext.User.Identity.GetUserId());
            if (!hasGroups)
            {
                return RedirectToAction("Index", "Groups");
            }

            this._service.PopulateGames(this.HttpContext.User.Identity.GetUserId());

            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Feedback(FeedbackBindingModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                this._service.CreateFeedback(bindingModel, this.HttpContext.User.Identity.GetUserId());

                return RedirectToAction("Dashboard");
            }

            return View(bindingModel);
        }
    }
}