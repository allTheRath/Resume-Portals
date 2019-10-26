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
    public class StudentController : Controller
    {

        private protected ApplicationDbContext db = new ApplicationDbContext();
        private protected RoleHandler RoleHandler = new RoleHandler();



        //public ActionResult StudentNav()
        //{
        //    return View();
        //}

        /// <summary>
        /// Student Home page
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        public ActionResult Student()
        {
            string userId = User.Identity.GetUserId();
            var student = db.Profiles.FirstOrDefault(s => s.UserId == userId);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role = "Student";

            //list of programs
            return View(student);
        }
        // Can add activities
        // Can update resume or profile
        // Can See all employers
        // Can see program home. :: which will have, program discription, all students of program and events 
        // Can't see other student profiles. Only partial profile will be visible in program home page.

        public ActionResult CreateStudent()
        {
            if (User.IsInRole("Student"))
            {
                return HttpNotFound();
            }
            ViewBag.Role = "Student";
            return View();
        }

        [HttpPost, ActionName("CreateStudent")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudentConfirm([Bind(Include = "Id,AboutMe,ContactInfo,ProfetionalEmail,SemesterNumber,StartDate,EndDate,MySkills")] StudentProfile studentProfile)
        {
            ViewBag.Role = "Student";
            if (ModelState.IsValid)
            {
                string uid = User.Identity.GetUserId();
                StudentProfile studentProfileExist = db.StudentProfiles.ToList().Where(x => x.UserId == uid).FirstOrDefault();
                studentProfileExist.AboutMe = studentProfile.AboutMe;
                studentProfileExist.ContactInfo = studentProfile.ContactInfo;
                studentProfileExist.EndDate = studentProfile.EndDate;
                studentProfileExist.MySkills = studentProfile.MySkills;
                studentProfileExist.ProfessionalEmail = studentProfile.ProfessionalEmail;
                studentProfileExist.SemesterNumber = studentProfile.SemesterNumber;
                studentProfileExist.StartDate = studentProfile.StartDate;
                db.SaveChanges();
                return RedirectToAction("Student");
            }

            return View(studentProfile);
        }



        public ActionResult EditStudent()
        {
            if (User.IsInRole("Student"))
            {
                return HttpNotFound();
            }
            string uid = User.Identity.GetUserId();
            StudentProfile studentProfile = db.StudentProfiles.ToList().Where(x => x.UserId == uid).FirstOrDefault();
            ViewBag.Role = "Student";

            return View(studentProfile);
        }

        [HttpPost, ActionName("CreateStudent")]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudentConfirm([Bind(Include = "Id,AboutMe,ContactInfo,ProfetionalEmail,SemesterNumber,StartDate,EndDate,MySkills")] StudentProfile studentProfile)
        {
            if (ModelState.IsValid)
            {
                string uid = User.Identity.GetUserId();
                StudentProfile studentProfileExist = db.StudentProfiles.ToList().Where(x => x.UserId == uid).FirstOrDefault();
                studentProfileExist.AboutMe = studentProfile.AboutMe;
                studentProfileExist.ContactInfo = studentProfile.ContactInfo;
                studentProfileExist.EndDate = studentProfile.EndDate;
                studentProfileExist.MySkills = studentProfile.MySkills;
                studentProfileExist.ProfessionalEmail = studentProfile.ProfessionalEmail;
                studentProfileExist.SemesterNumber = studentProfile.SemesterNumber;
                studentProfileExist.StartDate = studentProfile.StartDate;
                db.SaveChanges();
                return RedirectToAction("Student");
            }
            ViewBag.Role = "Student";
            return View(studentProfile);
        }


        /// <summary>
        /// Each student profile consist of only their details.
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            string uid = User.Identity.GetUserId();
            StudentProfile studentProfile = db.StudentProfiles.ToList().Where(x => x.UserId == uid).FirstOrDefault();
            bool exists = Directory.Exists(Server.MapPath("~/User-Profile-Pic/" + User.Identity.Name));
            if (exists && User.Identity.Name != "")
            {
                string ProfileImage = Directory.GetFiles(Server.MapPath("~/User-Profile-Pic/" + User.Identity.Name + "/"), "*.*", SearchOption.AllDirectories)[0];
                string fileextention = Path.GetExtension(ProfileImage).ToLower();
                ViewBag.ImageUrl = "/User-Profile-Pic/" + User.Identity.Name + "/" + "profilepic" + fileextention;
            }
            else
            {
                ViewBag.ImageUrl = "/User-Profile-Pic/" + "blank" + "/" + "blankProfile" + ".png";
            }
            ViewBag.Role = "Student";

            return View(studentProfile);
        }

        public ActionResult ViewExperience()
        {
            string userid = User.Identity.GetUserId();
            
            var experiances = db.Experiances.ToList().Where(x => x.UserId == userid).ToList();
            if (experiances == null)
            {
                experiances = new List<Experiance>();
            }

            return View(experiances);
        }

        public ActionResult ViewSkills()
        {
            string userid = User.Identity.GetUserId();
            var skills = db.Skills.ToList().Where(x => x.UserId == userid).ToList();
            if (skills == null)
            {
                skills = new List<Skill>();
            }

            return View(skills);
        }

        public ActionResult ViewEducation()
        {
            string userid = User.Identity.GetUserId();
            var educations = db.Educations.ToList().Where(x => x.UserId == userid).ToList();
            if (educations == null)
            {
                educations = new List<Education>();
            }

            return View(educations);
        }

        public ActionResult AddExperience()
        {
            ViewBag.Role = "Student";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExperience([Bind(Include = "Id,InstituteName,Discription,Start,End")] Experiance experiance)
        {
            experiance.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Experiances.Add(experiance);
                db.SaveChanges();
                return RedirectToAction("StudentProfile");
            }
            ViewBag.Role = "Student";
            return View(experiance);
        }

        public ActionResult DeleteExperience(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experiance experiance = db.Experiances.Find(id);
            if (experiance == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role = "Student";
            return View(experiance);
        }

        [HttpPost, ActionName("DeleteExperience")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteExperienceConfirmed(int id)
        {
            Experiance experiance = db.Experiances.Find(id);
            db.Experiances.Remove(experiance);
            db.SaveChanges();
            ViewBag.Role = "Student";
            return RedirectToAction("StudentProfile");
        }


        public ActionResult AddSkill()
        {
            ViewBag.Role = "Student";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSkill([Bind(Include = "Id,UserId,SkillName")] Skill skill)
        {
            skill.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Skills.Add(skill);
                db.SaveChanges();
                return RedirectToAction("StudentProfile");
            }

            ViewBag.Role = "Student";
            return View(skill);
        }



        public ActionResult DeleteSkill(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Skill skill = db.Skills.Find(id);
            if (skill == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role = "Student";
            return View(skill);
        }

        [HttpPost, ActionName("DeleteSkill")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSkillConfirmed(int id)
        {
            Skill skill = db.Skills.Find(id);
            db.Skills.Remove(skill);
            db.SaveChanges();
            ViewBag.Role = "Student";
            return RedirectToAction("StudentProfile");
        }



        public ActionResult AddEducation()
        {
            ViewBag.Role = "Student";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEducation([Bind(Include = "Id,UserId,InstituteName,Discription,Start,End")] Education education)
        {
            education.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Educations.Add(education);
                db.SaveChanges();
                return RedirectToAction("StudentProfile");
            }
            ViewBag.Role = "Student";
            return View(education);
        }

        public ActionResult DeleteEducation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role = "Student";
            return View(education);
        }

        [HttpPost, ActionName("DeleteEducation")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEducationConfirmed(int id)
        {
            Education education = db.Educations.Find(id);
            db.Educations.Remove(education);
            db.SaveChanges();
            ViewBag.Role = "Student";
            return RedirectToAction("Student");
        }

    }
}