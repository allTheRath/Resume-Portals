using Microsoft.AspNet.Identity;
using Resume_Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Resume_Portal.Controllers
{
    public class InstructorController : Controller
    {
        private protected ApplicationDbContext db = new ApplicationDbContext();
        private protected RoleHandler RoleHandler = new RoleHandler();


        //public ActionResult InstructorNav()
        //{
        //    return View();
        //}
        /// <summary>
        /// Instructor home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Instructor()
        {
            if (!User.IsInRole("Instructor"))
            {
                return HttpNotFound();
            }
            string userId = User.Identity.GetUserId();
            var instructor = db.Profiles.FirstOrDefault(s => s.UserId == userId);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role = "Instructor";

            return View(instructor);
        }

        public ActionResult CreateInstructor()
        {
            if (!User.IsInRole("Instructor"))
            {
                return HttpNotFound();
            }
            ViewBag.Role = "Instructor";

            return View();
        }

        [HttpPost, ActionName("CreateInstructor")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInstructorConfirm([Bind(Include = "Id,AboutMe,Experience,JoinedMitt,ContactInfo,ProfetionalEmail")] InstructorProfile instructorProfile)
        {

            if (ModelState.IsValid)
            {
                string uid = User.Identity.GetUserId();
                InstructorProfile instructorProfileExist = db.InstructorProfiles.ToList().Where(x => x.UserId == uid).FirstOrDefault();
                instructorProfileExist.AboutMe = instructorProfile.AboutMe;
                instructorProfileExist.ContactInfo = instructorProfile.ContactInfo;
                instructorProfileExist.Experience = instructorProfile.Experience;
                instructorProfileExist.JoinedMitt = instructorProfile.JoinedMitt;
                instructorProfileExist.ProfetionalEmail = instructorProfile.ProfetionalEmail;
                db.SaveChanges();

                return RedirectToAction("Instructor");
            }
            ViewBag.Role = "Instructor";

            return View(instructorProfile);
        }

        public ActionResult EditInstructor()
        {
            if (!User.IsInRole("Instructor"))
            {
                return HttpNotFound();
            }
            string uid = User.Identity.GetUserId();
            InstructorProfile instructorProfile = db.InstructorProfiles.ToList().Where(x => x.UserId == uid).FirstOrDefault();
            ViewBag.Role = "Instructor";

            return View(instructorProfile);
        }

        [HttpPost, ActionName("EditInstructor")]
        [ValidateAntiForgeryToken]
        public ActionResult EditInstructorConfirm([Bind(Include = "Id,AboutMe,Experience,JoinedMitt,ContactInfo,ProfetionalEmail")] InstructorProfile instructorProfile)
        {

            if (ModelState.IsValid)
            {
                string uid = User.Identity.GetUserId();
                InstructorProfile instructorProfileExist = db.InstructorProfiles.ToList().Where(x => x.UserId == uid).FirstOrDefault();
                instructorProfileExist.AboutMe = instructorProfile.AboutMe;
                instructorProfileExist.ContactInfo = instructorProfile.ContactInfo;
                instructorProfileExist.Experience = instructorProfile.Experience;
                instructorProfileExist.JoinedMitt = instructorProfile.JoinedMitt;
                instructorProfileExist.ProfetionalEmail = instructorProfile.ProfetionalEmail;
                db.SaveChanges();
                ViewBag.Role = "Instructor";

                return RedirectToAction("Instructor");
            }

            return View(instructorProfile);
        }
        // can see list of students.
        // Can see program home.
        // Can post events :: which will be displayed on program home page.
        // Can see all employees.
        // Can contact any student or employee.

        /// <summary>
        /// Instructor profile view ... Instructor can edit the profile.  
        /// </summary>
        /// <returns></returns>
        public ActionResult InstructorProfile()
        {
            if (User.IsInRole("Instructor"))
            {
                return HttpNotFound();
            }
            string uid = User.Identity.GetUserId();
            InstructorProfile instructorProfile = db.InstructorProfiles.Where(x => x.UserId == uid).FirstOrDefault();
            ViewBag.Role = "Instructor";

            return View(instructorProfile);
        }

    }
}