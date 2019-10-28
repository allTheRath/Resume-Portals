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
    public class EmployerController : Controller
    {

        private protected ApplicationDbContext db = new ApplicationDbContext();
        private protected RoleHandler RoleHandler = new RoleHandler();

        // GET: Employer
        //public ActionResult EmployerNav()
        //{
        //    return View();
        //}

        /// <summary>
        /// Employer Home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Employer()
        {
            if (!User.IsInRole("Employer"))
            {
                return HttpNotFound();
            }
            string userId = User.Identity.GetUserId();
            var employer = db.Profiles.FirstOrDefault(s => s.UserId == userId);
            if (employer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role = "Employer";
            return View(employer);
        }

        /// <summary>
        /// Employer profile view ... Employer can edit the profile.  
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployerProfile()
        {
            if (!User.IsInRole("Employer"))
            {
                return HttpNotFound();
            }
            string uid = User.Identity.GetUserId();
            EmployerProfile employerProfile = db.EmployerProfiles.ToList().Where(x => x.UserId == uid).FirstOrDefault();
            return View(employerProfile);
        }

        // Employer can see list of students or can see program home page.
        // Employer can send email to any student or instructor.
        // Student can see notification of sent email.
        public ActionResult EditEmployer()
        {
            string uid = User.Identity.GetUserId();
            EmployerProfile employerProfile = db.EmployerProfiles.Where(x => x.UserId == uid).FirstOrDefault();
            return View(employerProfile);
        }

        [HttpPost, ActionName("EditEmployer")]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployerConfirmation([Bind(Include = "Id,AboutUs,HistoryOfCompany,ContactUs,PhoneNo,LookingForSkills")] EmployerProfile employerProfile)
        {
            if (ModelState.IsValid)
            {

                string uid = User.Identity.GetUserId();
                EmployerProfile employerProfileExist = db.EmployerProfiles.Where(x => x.UserId == uid).FirstOrDefault();
                employerProfileExist.AboutUs = employerProfile.AboutUs;
                employerProfileExist.ContactUs = employerProfile.ContactUs;
                employerProfileExist.HistoryOfCompany = employerProfile.HistoryOfCompany;
                employerProfileExist.LookingForSkills = employerProfile.LookingForSkills;
                employerProfileExist.PhoneNo = employerProfile.PhoneNo;
                db.SaveChanges();
                return RedirectToAction("Employer");
            }

            return View(employerProfile);
        }


        public ActionResult CreateEmployer()
        {
            if (!User.IsInRole("Employer"))
            {
                return HttpNotFound();
            }
            return View();
        }


        [HttpPost, ActionName("CreateEmployer")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployerConfirmation([Bind(Include = "Id,AboutUs,HistoryOfCompany,ContactUs,PhoneNo,LookingForSkills")] EmployerProfile employerProfile)
        {
            if (ModelState.IsValid)
            {

                string uid = User.Identity.GetUserId();
                EmployerProfile employerProfileExist = db.EmployerProfiles.Where(x => x.UserId == uid).FirstOrDefault();
                employerProfileExist.AboutUs = employerProfile.AboutUs;
                employerProfileExist.ContactUs = employerProfile.ContactUs;
                employerProfileExist.HistoryOfCompany = employerProfile.HistoryOfCompany;
                employerProfileExist.LookingForSkills = employerProfile.LookingForSkills;
                employerProfileExist.PhoneNo = employerProfile.PhoneNo;
                db.SaveChanges();
                return RedirectToAction("Employer");
            }

            return View(employerProfile);
        }


        /// <summary>
        /// Employer can post job to a program. All students of that program will be able to see the job.
        /// </summary>
        /// <returns></returns>
       // [Authorize(Roles = "Employer")]
        public ActionResult PostJob()
        {
            if (ViewBag.PostConfirm != null && ViewBag.PostConfirm == true)
            {
                ViewBag.PostConfirm = true;
            }
            else
            {
                ViewBag.PostConfirm = false;
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Name");
            return View();
        }
        [HttpPost, ActionName("PostJob")]
        [ValidateAntiForgeryToken]
        public ActionResult PostJobConfirmation([Bind(Include = "CompanyName, JobDiscription")] Job job)
        {
            string uid = User.Identity.GetUserId();
            job.EmployerId = uid;
            job.PostedOn = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                db.SaveChanges();
                ViewBag.PostConfirm = true;
            }
            else
            {
                ViewBag.PostConfirm = false;
            }
            ViewBag.ProgramId = new SelectList(db.Programs, "Id", "Name");
            return View();
        }


    }
}