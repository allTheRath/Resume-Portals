using Resume_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Resume_Portal.Controllers
{
    public class AdminController : Controller
    {
        private protected ApplicationDbContext db = new ApplicationDbContext();
        private protected RoleHandler RoleHandler = new RoleHandler();


        public ActionResult AdminNav()
        {
            return View();
        }


        /// <summary>
        /// Admin Home Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin()
        {
            // On landing all users that are online will be seen.
            var userDetails = db.Users.ToList().Where(x => x.Online == true).ToList();
            if (userDetails.Count() == 0)
            {
                userDetails = new List<ApplicationUser>();
            }
            ViewBag.Role = "Admin";
            return View(userDetails);
        }

        // Admin and instructor can assign students and employers.
        /// <summary>
        ///  Multiple users can be assigned to appropriate roles. Filter user by requested roles roles
        /// </summary>
        /// <returns></returns>
        public ActionResult AssignUsers()
        {
            if (!User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var unassignedUsers = db.NotifyAdmins.ToList().Where(x => x.Resolved == false).ToList();
            if (unassignedUsers.Count() == 0)
            {
                unassignedUsers = new List<NotifyAdmin>();
            }
            ViewBag.Role = "Admin";
            return View(unassignedUsers);
        }

        public ActionResult AssignUsersConfirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = db.NotifyAdmins.Find(id);
            ViewBag.RoleName = request.RoleName;
            if (request.Resolved == false)
            {
                bool approved = RoleHandler.AssignUserToRole(request.UserId, request.RoleName);
                if (approved == true)
                {
                    ViewBag.Approved = true;
                }
                else
                {
                    ViewBag.Approved = false;
                }
            }
            var user = db.Users.Find(request.UserId);
            ViewBag.Role = "Admin";
            return View(user);
        }

        /// <summary>
        ///  Multiple users can be un assigned to a role.
        /// </summary>
        /// <returns></returns>
        public ActionResult UnAssignUsers()
        {
            if (!User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var assignedUsers = db.NotifyAdmins.ToList().Where(x => x.Resolved == true).ToList();
            if (assignedUsers.Count() == 0)
            {
                assignedUsers = new List<NotifyAdmin>();
            }
            ViewBag.Role = "Admin";
            return View(assignedUsers);
        }

        public ActionResult UnAssignUsersConfirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var request = db.NotifyAdmins.Find(id);
            ViewBag.Approved = false;
            ViewBag.RoleName = request.RoleName;
            string userID = request.UserId;
            if (request.Resolved == true)
            {
                bool approved = RoleHandler.UnassignUserToRole(request.UserId, request.RoleName);
                if (approved == true)
                {
                    db.NotifyAdmins.Remove(request);
                    db.SaveChanges();
                    ViewBag.Approved = true;
                }
            }
            var user = db.Users.Find(userID);
            ViewBag.Role = "Admin";
            return View(user);
        }

    }
}