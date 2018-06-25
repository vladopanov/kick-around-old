using System.Collections.Generic;
using System.Web.Mvc;
using KickAround.Models.ViewModels.Notifications;
using KickAround.Services.Contracts;
using Microsoft.AspNet.Identity;

namespace KickAround.Web.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationsService _service;

        public NotificationsController(INotificationsService service)
        {
            this._service = service;
        }

        // GET: Notifications
        public ActionResult Index()
        {
            IEnumerable<NotificationViewModel> notifications =
                this._service.GetUserNotifications(this.HttpContext.User.Identity.GetUserId());

            return View(notifications);
        }

        public ActionResult Menu()
        {
            int unreadNotificationsCount =
                this._service.GetNumberOfUnreadNotifications(this.HttpContext.User.Identity.GetUserId());

            ViewBag.NumberOfUnreadNotifications = unreadNotificationsCount;
            return PartialView("_NotificationsPartial");
        }

        public ActionResult Accept(string id)
        {
            this._service.AcceptJoinGroupRequest(id, this.HttpContext.User.Identity.GetUserId());

            return RedirectToAction("Index");
        }

        public ActionResult Reject(string id)
        {
            this._service.RejectJoinGroupRequest(id, this.HttpContext.User.Identity.GetUserId());

            return RedirectToAction("Index");
        }

        public ActionResult Read(string id)
        {
            this._service.ReadNotification(id);

            return RedirectToAction("Index");
        }

        public ActionResult ReadAll()
        {
            this._service.MarkAllNotificationAsRead(this.HttpContext.User.Identity.GetUserId());

            return RedirectToAction("Index");
        }
    }
}