using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using KickAround.Models.EntityModels;
using KickAround.Services.Contracts;

namespace KickAround.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeedbacksController : Controller
    {
        private readonly IFeedbacksService _service;

        public FeedbacksController(IFeedbacksService service)
        {
            this._service = service;
        }


        // GET: Admin/Feedbacks
        public ActionResult Index()
        {
            IEnumerable<Feedback> feedbacks = this._service.GetAllFeedbacks();

            return View(feedbacks);
        }

        // GET: Admin/Feedbacks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = this._service.GetFeedback(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Admin/Feedbacks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = this._service.GetFeedback(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Admin/Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this._service.DeleteFeedback(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._service.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
