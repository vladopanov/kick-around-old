using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KickAround.Models.EntityModels;
using KickAround.Models.ViewModels.Users;
using KickAround.Services;
using KickAround.Web.Utilities;
using Microsoft.AspNet.Identity;

namespace KickAround.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersService _service;

        BlobUtility utility;
        string accountName = "kickaround";
        string accountKey = "2K8l2K3WgTwG2KL4J219rs5JC/AeePZ8Dpe/NfnhIXMdee0bUXSDDKZUAqd2eZpt2grn76IhJ4cvJ9RlmRKVcg==";

        public UsersController()
        {
            this._service = new UsersService();
            this.utility = new BlobUtility(accountName, accountKey);
        }

        public ActionResult Picture()
        {
            string loggedInUserId = User.Identity.GetUserId();
            List<UserImage> userImages = (from r in this._service.Context.UserImages where r.UserId == loggedInUserId select r).ToList();
            ViewBag.PhotoCount = userImages.Count;
            return View(userImages);
        }

        // GET: Users
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserDetailsViewModel user = this._service.GetUserDetails(id);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            return View(user);
        }

        public ActionResult DeleteImage(string id)
        {
            UserImage userImage = this._service.DeleteImage(id);
            string BlobNameToDelete = userImage.ImageUrl.Split('/').Last();
            utility.DeleteBlob(BlobNameToDelete, "userimages");
            return RedirectToAction("Picture");
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string ContainerName = "userimages"; //hardcoded container name. 
                file = file ?? Request.Files["file"];
                string fileName = Path.GetFileName(file.FileName);
                Stream imageStream = file.InputStream;
                var result = utility.UploadBlob(fileName, ContainerName, imageStream);
                if (result != null)
                {
                    string loggedInUserId = User.Identity.GetUserId();

                    User user = this._service.Context.Users.Find(loggedInUserId);
                    if (user.UserImages.Count > 0)
                    {
                        string imageId = user.UserImages.ToList()[0].Id;

                        UserImage userImage = this._service.DeleteImage(imageId);
                        string BlobNameToDelete = userImage.ImageUrl.Split('/').Last();
                        utility.DeleteBlob(BlobNameToDelete, "userimages");
                    }

                    UserImage userimage = new UserImage();
                    userimage.Id = Guid.NewGuid().ToString();
                    userimage.UserId = loggedInUserId;
                    userimage.ImageUrl = result.Uri.ToString();
                    this._service.Context.UserImages.Add(userimage);
                    this._service.Context.SaveChanges();
                    return RedirectToAction("Picture");
                }
                else
                {
                    return RedirectToAction("Picture");
                }
            }
            else
            {
                return RedirectToAction("Picture");
            }
        }
    }
}