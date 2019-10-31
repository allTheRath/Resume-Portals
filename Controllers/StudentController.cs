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



        public ActionResult StudentNav()
        {
            return View();
        }

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
            ViewBag.ProfilePic = student.ProfilePic;
            //list of programs
            return View(student);
        }

        public ActionResult EditProfile(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Profile profile = db.Profiles.Find(id);

            ViewBag.Role = "Student";
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
            ViewBag.Role = "Student";

            return RedirectToAction("Student");
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
        public ActionResult ProfileView()
        {
            return View();
        }
        [ActionName("StudentDetails")]
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
        //[ActionName("KeeganProfile")]
        //public ActionResult StudentProfile(string id)
        //{
        //    var profile = db.Profiles.Where(p => p.UserId == id).FirstOrDefault();
        //    if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
        //    {
        //        id = User.Identity.GetUserId();
        //        profile = db.Profiles.FirstOrDefault(p => p.UserId == id);
        //    }
        //    if (profile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var CompleteProfile = db.StudentProfiles.Where(x => x.UserId == id).FirstOrDefault();

        //    CompleteStudentInfo completeStudent = new CompleteStudentInfo();
        //    completeStudent.AboutMe = CompleteProfile.AboutMe;
        //    completeStudent.Activities = db.Activities.Where(x => x.UserId == id).ToList();
        //    if (completeStudent.Activities == null)
        //    {
        //        completeStudent.Activities = new List<Activity>();
        //    }
        //    completeStudent.Attachments = db.Attachments.Where(x => x.StudentId == id).ToList();
        //    if (completeStudent.Attachments == null)
        //    {
        //        completeStudent.Attachments = new List<Attachment>();
        //    }
        //    completeStudent.ContactInfo = CompleteProfile.ContactInfo;
        //    completeStudent.Educations = db.Educations.Where(x => x.UserId == id).ToList();
        //    if (completeStudent.Educations == null)
        //    {
        //        completeStudent.Educations = new List<Education>();
        //    }
        //    completeStudent.EndDate = CompleteProfile.EndDate;
        //    completeStudent.Experiances = db.Experiances.Where(x => x.UserId == id).ToList();
        //    if (completeStudent.Experiances == null)
        //    {
        //        completeStudent.Experiances = new List<Experiance>();
        //    }
        //    completeStudent.Skills = db.Skills.Where(x => x.UserId == id).ToList();
        //    if (completeStudent.Skills == null)
        //    {
        //        completeStudent.Skills = new List<Skill>();
        //    }
        //    completeStudent.ProfileId = CompleteProfile.Id;
        //    completeStudent.MyName = CompleteProfile.MyName;
        //    completeStudent.OccupationName = CompleteProfile.OccupationName;
        //    completeStudent.ProfessionalEmail = CompleteProfile.ProfessionalEmail;
        //    completeStudent.ProfilePicUrl = profile.ProfilePic;
        //    completeStudent.SemesterNumber = CompleteProfile.SemesterNumber;
        //    List<Event> Valentearings = new List<Event>();
        //    var eventIds = db.EventParticipatedStudents.Where(x => x.Student_profileId == CompleteProfile.Id).ToList().Select(x => x.Event_Id);
        //    if (eventIds != null)
        //    {
        //        foreach (var eId in eventIds)
        //        {
        //            Event e = db.Events.Find(eId);
        //            Valentearings.Add(e);
        //        }
        //    }
        //    completeStudent.Volenteering = Valentearings;
        //    completeStudent.SortDiscription = profile.ShortDiscription;
        //    completeStudent.StudentId = id;

        //    return View(completeStudent);
        //}
        public ActionResult profileExperince(CompleteStudentInfo studentInfo)
        {
            return View(studentInfo);
        }
        public ActionResult profileEducation(CompleteStudentInfo studentInfo)
        {
            return View(studentInfo);
        }
        public ActionResult profileSkills(CompleteStudentInfo studentInfo)
        {
            return View(studentInfo);
        }
        [ActionName("KeeganProfile")]
        public ActionResult StudentProfile(string id, string subContentUrl)
        {
            if (!string.IsNullOrEmpty(subContentUrl) && !string.IsNullOrWhiteSpace(subContentUrl))
            {
                ViewBag.url = subContentUrl;
                switch (subContentUrl)
                {
                    case "profileSkills":
                        ViewBag.skills = "active";
                        break;
                    case "profileEducation":
                        ViewBag.Education = "active";
                        break;
                    case "profileExperince":
                        ViewBag.Education = "active";
                        break;
                }

            }
            else
            {
                ViewBag.url = "profileSkills";
                ViewBag.skills = "active";
            }
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                id = User.Identity.GetUserId();
            }
            var profile = db.Profiles.Where(p => p.UserId == id).FirstOrDefault();
            var role = RoleHandler.GetUserRole(id);
            if(!string.IsNullOrWhiteSpace(role) && !string.IsNullOrEmpty(id))
            {
                ViewBag.Role = role;
            }
            if (profile == null)
            {
                return HttpNotFound();
            }
            var CompleteProfile = db.StudentProfiles.Where(x => x.UserId == id).FirstOrDefault();

            CompleteStudentInfo completeStudent = new CompleteStudentInfo();
            completeStudent.AboutMe = CompleteProfile.AboutMe;
            completeStudent.Activities = db.Activities.Where(x => x.UserId == id).ToList();
            if (completeStudent.Activities == null)
            {
                completeStudent.Activities = new List<Activity>();
            }
            completeStudent.Attachments = db.Attachments.Where(x => x.StudentId == id).ToList();
            if (completeStudent.Attachments == null)
            {
                completeStudent.Attachments = new List<Attachment>();
            }
            completeStudent.ContactInfo = CompleteProfile.ContactInfo;
            completeStudent.Educations = db.Educations.Where(x => x.UserId == id).ToList();
            if (completeStudent.Educations == null)
            {
                completeStudent.Educations = new List<Education>();
            }
            completeStudent.EndDate = CompleteProfile.EndDate;
            completeStudent.Experiances = db.Experiances.Where(x => x.UserId == id).ToList();
            if (completeStudent.Experiances == null)
            {
                completeStudent.Experiances = new List<Experiance>();
            }
            completeStudent.Skills = db.Skills.Where(x => x.UserId == id).ToList();
            if (completeStudent.Skills == null)
            {
                completeStudent.Skills = new List<Skill>();
            }
            completeStudent.ProfileId = CompleteProfile.Id;
            completeStudent.MyName = CompleteProfile.MyName;
            completeStudent.OccupationName = CompleteProfile.OccupationName;
            completeStudent.ProfessionalEmail = CompleteProfile.ProfessionalEmail;
            completeStudent.ProfilePicUrl = profile.ProfilePic;
            completeStudent.SemesterNumber = CompleteProfile.SemesterNumber;
            List<Event> Valentearings = new List<Event>();
            var eventIds = db.EventParticipatedStudents.Where(x => x.Student_profileId == CompleteProfile.Id).ToList().Select(x => x.Event_Id);
            if (eventIds != null)
            {
                foreach (var eId in eventIds)
                {
                    Event e = db.Events.Find(eId);
                    Valentearings.Add(e);
                }
            }
            completeStudent.Volenteering = Valentearings;
            completeStudent.SortDiscription = profile.ShortDiscription;
            completeStudent.StudentId = id;

            return View(completeStudent);
        }

        public ActionResult ViewExperience()
        {
            string userid = User.Identity.GetUserId();
            var experiances = db.Experiances.ToList().Where(x => x.UserId == userid).ToList();
            if (experiances == null)
            {
                experiances = new List<Experiance>();
            }
            ViewBag.Role = "Student";

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
            ViewBag.Role = "Student";

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
            ViewBag.Role = "Student";

            return View(educations);
        }


        public ActionResult UpdateProfilePic()
        {

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


            return RedirectToAction("Student");
        }

        public ActionResult Download()
        {
            string uid = User.Identity.GetUserId();
            bool exist = Directory.Exists(Server.MapPath("~/student-resume/" + uid + "/"));
            if (exist != false)
            {
                var allFileNames = Directory.GetFiles(Server.MapPath("~/student-resume/" + uid + "/"));
                if (allFileNames.Length > 0)
                {

                    var fileName = Path.GetFileName(allFileNames[0]);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/student-resume/" + uid + "/" + fileName));
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }
        }


        public ActionResult UploadMyResume()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadMyResume(HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                var fileextention = Path.GetExtension(file.FileName).ToLower();
                if (fileextention == ".pdf" || fileextention == ".docx" || fileextention == ".docm" || fileextention == ".dotx" || fileextention == ".dotm" || fileextention == ".docb" || fileextention == ".rtf")
                {
                    string uid = User.Identity.GetUserId();
                    if (uid != "")
                    {
                        bool exists = Directory.Exists(Server.MapPath("~/student-resume/" + uid));
                        if (!exists)
                        {
                            // if directory not exist then create it.
                            Directory.CreateDirectory(Server.MapPath("~/student-resume/" + uid));
                        }
                        if (System.IO.File.Exists(Server.MapPath("~/student-resume/" + uid + "/" + "resume" + fileextention)))
                        {
                            // if file exist then delete it.
                            System.IO.File.Delete(Server.MapPath("~/student-resume/" + uid + "/" + "resume" + fileextention));
                        }
                        var path = Path.Combine(Server.MapPath("~/student-resume/" + uid + "/"), "resume" + fileextention);
                        file.SaveAs(path);

                    }
                }
            }


            return RedirectToAction("Student");
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


        /// <summary>
        /// All activities for a single student
        /// </summary>
        /// <returns></returns>
        public ActionResult MyActivities()
        {
            var activities = db.Activities.ToList().Where(x => x.UserId == User.Identity.GetUserId());
            if (activities == null)
            {
                activities = new List<Activity>();

            }
            ViewBag.Role = "Student";
            return View(activities);
        }

        public ActionResult AddActivity()
        {
            if (!User.IsInRole("Student"))
            {
                return HttpNotFound();
            }
            string url = "";
            string uid = User.Identity.GetUserId();
            bool exists = Directory.Exists(Server.MapPath("~/Activity-Data/" + uid));
            if (!exists)
            {
                // if directory not exist then create it.
                Directory.CreateDirectory(Server.MapPath("~/Activity-Data/" + uid));
                url = "1";
            }

            string fileName = "";
            try
            {
                var allFileNames = Directory.GetFiles(Server.MapPath("~/Activity-Data/" + uid + "/"));
                if (allFileNames != null)
                {

                    fileName = allFileNames[allFileNames.Length - 1];
                }
                if (fileName != "")
                {
                    var number = Path.GetFileNameWithoutExtension(fileName);
                    url = (Convert.ToInt32(number) + 1).ToString();
                }
                else
                {
                    url = "1";
                }

            }
            catch (Exception)
            {
                url = "1";

            }

            ViewBag.Role = "Student";
            TempData.Add(uid, url);
            return View();
        }

        [HttpPost]
        public ActionResult AddActivity([Bind(Include = "Id,Discription,ActivityType")] Activity activity, HttpPostedFileBase file)
        {

            string uid = User.Identity.GetUserId();
            activity.UserId = uid;

            
            if (file.ContentLength > 0)
            {
                var fileextention = Path.GetExtension(file.FileName).ToLower();

                if (fileextention == ".jpg" || fileextention == ".jpeg" || fileextention == ".bmp" || fileextention == ".png")
                {
                    if (uid != "")
                    {
                        var path = Path.Combine(Server.MapPath("~/Activity-Data/" + uid + "/"), TempData[uid].ToString() + fileextention);
                        file.SaveAs(path);
                        activity.ImageUrl = "/Activity-Data/" + uid + "/" + TempData[uid].ToString() + fileextention;
                    }

                }
            }

            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();
            }
            ViewBag.Role = "Student";
            return View();

        }

        public ActionResult DeleteActivity(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Student");
            }


            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return RedirectToAction("Student");
            }

            if (activity.UserId != User.Identity.GetUserId())
            {
                return RedirectToAction("Student");
            }
            System.IO.File.Delete(Server.MapPath(activity.ImageUrl));
            db.Activities.Remove(activity);
            db.SaveChanges();

            return RedirectToAction("MyActivities");
        }

    }
}