using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using KickAround.Models.BindingModels.Groups;
using KickAround.Models.EntityModels;
using KickAround.Models.ViewModels.Groups;
using KickAround.Services.Contracts;
using Microsoft.AspNet.Identity;
using PagedList;

namespace KickAround.Web.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly IGroupsService _service;

        public GroupsController(IGroupsService service)
        {
            this._service = service;
        }

        // GET: Groups
        public ActionResult Index()
        {
            string userId = HttpContext.User.Identity.GetUserId();
            IEnumerable<GroupViewModel> allGroups = this._service.GetAllGroups(userId);

            return View(allGroups);
        }

        // GET: Groups/Details/5
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = this._service.GetGroup(id);
            if (group == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var groupDetails = this._service.GetGroupDetails(id, this.HttpContext.User.Identity.GetUserId());

            return View(groupDetails);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, Sport, Country, Town, Address")] CreateGroupBindingModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.User.Identity.GetUserId();
                this._service.CreateGroup(bindingModel, userId);

                return RedirectToAction("Index");
            }

            return View(bindingModel);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = this._service.GetGroup(id);
            if (group == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (!this._service.IsUserAuthorizedToModifyGroup(this.HttpContext.User.Identity.GetUserId(), group))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var groupToEdit = this._service.GetEditGroupModel(id);

            return View(groupToEdit);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Name, Sport, Country, Town, Address")] EditGroupBindingModel bindingModel)
        {
            if (ModelState.IsValid)
            {
                this._service.EditGroup(bindingModel);

                return RedirectToAction("Index");
            }
            return View(bindingModel);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Group group = this._service.GetGroup(id);
            if (group == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (!this._service.IsUserAuthorizedToModifyGroup(this.HttpContext.User.Identity.GetUserId(), group))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            this._service.DeleteGroup(id);

            return RedirectToAction("Index");
        }

        public ActionResult Join(string id)
        {
            this._service.JoinGroupRequest(id, this.HttpContext.User.Identity.GetUserId());

            return RedirectToAction("Index", new { id });
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult SearchQuery(string query, int? page)
        {
            if (query != null)
            {
                ViewBag.Query = query;
            }
            var groups = this._service.SearchGroups(query);

            var pageNumber = page ?? 1;
            var onePageOfGroups = groups.ToPagedList(pageNumber, 10);

            return PartialView("_SearchResultsPartial", onePageOfGroups);
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
