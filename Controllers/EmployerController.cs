﻿using Microsoft.AspNet.Identity;
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
        public ActionResult EmployerNav()
        {
            return View();
        }

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
            if (employer.ProfilePic == null || employer.ProfilePic == "")
            {
                employer.ProfilePic = "/User-Profile-Pic/blank/blankProfile.png";
                db.SaveChanges();
            }

            ViewBag.ProfilePic = employer.ProfilePic;
            return View(employer);
        }

        public ActionResult EditProfile(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Profile profile = db.Profiles.Find(id);

            ViewBag.Role = "Employer";
            return View(profile);
        }

        [HttpPost, ActionName("EditProfile")]
        public ActionResult EditProfileConfirm([Bind(Include = "Id,UserName,Email,ShortDiscription")] Profile profile)
        {
            Profile profileExist = db.Profiles.Find(profile.Id);
            profileExist.Email = profile.Email;
            profileExist.ShortDiscription = profile.ShortDiscription;
            profileExist.UserName = profile.UserName;
            db.SaveChanges();
            ViewBag.Role = "Employer";

            return RedirectToAction("Employer");
        }


        public ActionResult UpdateProfilePic()
        {
            ViewBag.Role = "Employer";
            return View();
        }
        [HttpPost]
        public ActionResult UpdateProfilePic(HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0)
            {
                var fileextention = Path.GetExtension(file.FileName).ToLower();
                if (fileextention == ".jpg" || fileextention == ".jpeg" || fileextention == ".bmp" || fileextention == ".png")
                {
                    string uid = User.Identity.Name;
                    if (uid != "")
                    {
                        bool exists = Directory.Exists(Server.MapPath("~/User-Profile-Pic/" + uid));
                        if (!exists)
                        {
                            // if directory not exist then create it.
                            Directory.CreateDirectory(Server.MapPath("~/User-Profile-Pic/" + uid));
                        }
                        if (System.IO.File.Exists(Server.MapPath("~/User-Profile-Pic/" + uid + "/" + "profilepic" + fileextention)))
                        {
                            // if file exist then delete it.
                            System.IO.File.Delete(Server.MapPath("~/User-Profile-Pic/" + uid + "/" + "profilepic" + fileextention));
                        }
                        var path = Path.Combine(Server.MapPath("~/User-Profile-Pic/" + uid + "/"), "profilepic" + fileextention);
                        file.SaveAs(path);
                        string userid = User.Identity.GetUserId();
                        Profile profile = db.Profiles.Where(x => x.UserId == userid).FirstOrDefault();
                        if (profile != null)
                        {
                            profile.ProfilePic = "/User-Profile-Pic/" + uid + "/profilepic" + fileextention;
                            db.SaveChanges();
                        }
                    }

                }
            }

            ViewBag.Role = "Employer";
            return RedirectToAction("Employer");
        }
        public ActionResult KeeganEmployer(string id)
        {
            string uid = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrWhiteSpace(id))
            {
                uid = id;
            }
            EmployerProfile employerProfile = db.EmployerProfiles.Where(x => x.UserId == uid).FirstOrDefault();
            Profile profile = db.Profiles.FirstOrDefault(p => p.UserId == uid);
            EmployerProfileViewModels profileViewModel = new EmployerProfileViewModels { profile = profile, EmployerProfile = employerProfile };
            return View(profileViewModel);
        }

        /// <summary>
        /// Employer profile view ... Employer can edit the profile.  
        /// </summary>
        /// <returns></returns>
        public ActionResult ProfileView()
        {
            return View();
        }
        public ActionResult EmployerProfile()
        {
            if (!User.IsInRole("Employer"))
            {
                return HttpNotFound();
            }
            string uid = User.Identity.GetUserId();
            EmployerProfile employerProfile = db.EmployerProfiles.ToList().Where(x => x.UserId == uid).FirstOrDefault();
            ViewBag.Role = "Employer";
            Profile p = db.Profiles.Where(x => x.UserId == uid).FirstOrDefault();
            string extention = Path.GetExtension(Server.MapPath(p.ProfilePic));
            string directoryName = db.Users.Find(uid).UserName;
            ViewBag.ProfilePic = "/User-Profile-Pic/" + directoryName + "/profilepic" + extention;
            return View(employerProfile);
        }

        // Employer can see list of students or can see program home page.
        // Employer can send email to any student or instructor.
        // Student can see notification of sent email.
        public ActionResult EditEmployer()
        {
            string uid = User.Identity.GetUserId();
            EmployerProfile employerProfile = db.EmployerProfiles.Where(x => x.UserId == uid).FirstOrDefault();
            ViewBag.Role = "Employer";
            Profile p = db.Profiles.Where(x => x.UserId == uid).FirstOrDefault();
            string extention = Path.GetExtension(Server.MapPath(p.ProfilePic));
            string directoryName = db.Users.Find(uid).UserName;
            ViewBag.ProfilePic = "/User-Profile-Pic/" + directoryName + "/profilepic" + extention;
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
            ViewBag.Role = "Employer";
            return View(employerProfile);
        }


        public ActionResult CreateEmployer()
        {
            if (!User.IsInRole("Employer"))
            {
                return HttpNotFound();
            }
            ViewBag.Role = "Employer";
            string uid = User.Identity.GetUserId();
            Profile p = db.Profiles.Where(x => x.UserId == uid).FirstOrDefault();
            string extention = Path.GetExtension(Server.MapPath(p.ProfilePic));
            string directoryName = db.Users.Find(uid).UserName;
            ViewBag.ProfilePic = "/User-Profile-Pic/" + directoryName + "/profilepic" + extention;
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
            ViewBag.Role = "Employer";
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
            ViewBag.Role = "Employer";

            return View();
        }
        [HttpPost, ActionName("PostJob")]
        [ValidateAntiForgeryToken]
        public ActionResult PostJobConfirmation([Bind(Include = "CompanyName, JobDiscription, ProgramId")] Job job)
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
            ViewBag.Role = "Employer";
            return View();
        }

        public ActionResult MyPostedJobs()
        {
            var jobs = db.Jobs.ToList().Where(x => x.EmployerId == User.Identity.GetUserId());
            if (jobs == null)
            {
                jobs = new List<Job>();
            }

            return View(jobs);
        }
    }
}