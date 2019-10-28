using Microsoft.AspNet.Identity;
using Resume_Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Resume_Portal.Controllers
{
    public class HomeController : Controller
    {
        private protected ApplicationDbContext db = new ApplicationDbContext();
        private protected RoleHandler RoleHandler = new RoleHandler();


        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UserDirect");
                // If user is already loged in then user should go to their own home page. here profile page.
            }


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UserDirect()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // If any user try to access this page without login, for security purpose this check is done.
                return RedirectToAction("Index");
            }
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Admin", "Admin");
            }
            else if (User.IsInRole("Employer"))
            {
                return RedirectToAction("Employer", "Employer");
            }
            else if (User.IsInRole("Instructor"))
            {
                return RedirectToAction("Instructor", "Instructor");
            }
            else if (User.IsInRole("Student"))
            {
                return RedirectToAction("Student", "Student");
            }

            return View();
        }

        // Below are the navigation bar for individual users.





        public ActionResult ProfilePic()
        {
            return View();
        }

        public ActionResult ResumeUpload()
        {
            return View();
        }

        public ActionResult ResumeDownload()
        {
            return View();
        }

        public ActionResult ResumeViewer()
        {
            return View();
        }

        public ActionResult FooterBar()
        {
            return View();
        }






        public class ProgramSelect
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public ActionResult SelectProgram()
        {
            var allProgramNameId = db.Programs.ToList();
            List<ProgramSelect> programSelects = new List<ProgramSelect>();
            foreach (var p in allProgramNameId)
            {
                ProgramSelect programSelect = new ProgramSelect();
                programSelect.Id = p.Id;
                programSelect.Name = p.Name;
                programSelects.Add(programSelect);
            }
            ViewBag.ProgramId = programSelects;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectProgram(int Id)
        {
            if (Id < 0)
            {
                var allProgramNameId = db.Programs.ToList();
                List<ProgramSelect> programSelects = new List<ProgramSelect>();
                foreach (var p in allProgramNameId)
                {
                    ProgramSelect programSelect = new ProgramSelect();
                    programSelect.Id = p.Id;
                    programSelect.Name = p.Name;
                    programSelects.Add(programSelect);
                }
                ViewBag.ProgramId = programSelects;
                return View();
            }

            return RedirectToAction("AllStudents", new { Id = Id });
        }

        public ActionResult JobDetails(int? jobId)
        {
            if (jobId == null)
            {
                return HttpNotFound();
            }
            var job = db.Jobs.FirstOrDefault(j => j.Id == jobId);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }


        // Below Views are common for more then one role.
        //Ex: Admin ,instructor and Employer can see list of student with few details of students. 
        // Employer , admin and instructors can see this.

        /// <summary>
        /// List of all Employers 
        /// </summary>
        /// <returns></returns>
        public ActionResult AllEmployers()
        {
            var allEmployerProfiles = db.Profiles.ToList().Where(x => x.Role == "Employer").ToList();
            return View(allEmployerProfiles);
        }

        /// <summary>
        ///   List of all instructors
        /// </summary>
        /// <returns></returns>
        public ActionResult AllInstructors()
        {
            var allInstructors = db.Profiles.ToList().Where(x => x.Role == "Instructor").ToList();
            return View(allInstructors);
        }

        /// <summary>
        /// All students in College ,Seperated by programs.
        /// </summary>
        /// <returns></returns>
        public ActionResult AllStudents(int? Id)
        {

            if (Id == null)
            {
                return HttpNotFound();
            }
            var allUserOfProgram = db.ProgramUsers.ToList().Where(x => x.ProgramId == Id).Select(x => x.UserId);
            var allStudents = db.Profiles.ToList().Where(x => x.Role == "Student" && allUserOfProgram.Contains(x.UserId)).ToList();
            return View(allStudents);
        }
        //Profiles have a virtual card
        public ActionResult ProfileCard()
        {
            var profiles = db.Profiles.ToList();
            return View(profiles);
        }



        /// <summary>
        /// All programs offered :: List with images and headers. A link to program discription.
        /// </summary>
        /// <returns></returns>
        public ActionResult AllPrograms()
        {
            var allPrograms = db.Programs.ToList();

            allPrograms.ForEach(p =>
            {

                if (System.IO.File.Exists(Server.MapPath("~/program-images/" + p.Name + "/pic/logo.jpg")))
                {
                    p.ImageUrl = "/program-images/" + p.Name + "/pic/logo.jpg";
                } else
                {
                    p.ImageUrl = "/program-images/" + "Hospitality Management Diploma"+ "/pic/logo.jpg";
                }
                p.Discription = p.Discription.Substring(0, 450);
            });
            db.SaveChanges();
            if (allPrograms.Count() == 0)
            {
                allPrograms = new List<Program>();
            }

            return View(allPrograms);
        }

        /// <summary>
        /// Program discription based on program id // A below view will be list of users/Instructors/Students/Employers in this program 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProgramDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Program program = db.Programs.Find(id);
            return View(program);
        }

        /// <summary>
        /// Student list by program id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult StudentInProgram(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Program program = db.Programs.Find(id);
            var allProgramUsers = db.ProgramUsers.Where(x => x.ProgramId == id);
            List<Profile> allStudents = new List<Profile>();
            allProgramUsers.ToList().ForEach(user =>
            {
                var student = db.Profiles.Where(x => x.UserId == user.UserId && x.Role == "Student").FirstOrDefault();
                if (student != null)
                {
                    allStudents.Add(student);
                }
            });
            // all students in given program are retrived as list of profiles.
            return View(allStudents);
        }

        public ActionResult AllPostedJobOfProgram(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var jobs = db.Jobs.ToList().Where(x => x.ProgramId == id);
            if (jobs == null)
            {
                jobs = new List<Job>();
            }

            foreach (var job in jobs)
            {
                job.Employer = db.EmployerProfiles.Find(job.EmployerId);
            }
            return View(jobs);
        }

        public ActionResult UploadResume()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadResume(HttpPostedFileBase file, int? jobId)
        {

            if (file.ContentLength > 0 && jobId != null)
            {
                var fileextention = Path.GetExtension(file.FileName).ToLower();
                if (fileextention == ".pdf" || fileextention == ".docx" || fileextention == ".docm" || fileextention == ".dotx" || fileextention == ".dotm" || fileextention == ".docb" || fileextention == ".rtf")
                {
                    string uid = User.Identity.GetUserId();
                    if (uid != "")
                    {
                        bool exists = Directory.Exists(Server.MapPath("~/Posted-Job-Resume/" + jobId + "/"));
                        if (!exists)
                        {
                            // if directory not exist then create it.
                            Directory.CreateDirectory(Server.MapPath("~/Posted-Job-Resume/" + jobId + "/"));
                        }
                        if (System.IO.File.Exists(Server.MapPath("~/Posted-Job-Resume/" + jobId + "/" + User.Identity.Name + fileextention)))
                        {
                            // if file exist then delete it.
                            System.IO.File.Delete(Server.MapPath("~/Posted-Job-Resume/" + jobId + "/" + User.Identity.Name + fileextention));
                        }
                        var path = Path.Combine(Server.MapPath("~/Posted-Job-Resume/" + jobId + "/" + User.Identity.Name + fileextention));
                        file.SaveAs(path);
                    }
                }

            }


            return View();
        }

        public class FileWithPath
        {
            public string FileName { get; set; }
            public string fileUrl { get; set; }
        }

        public ActionResult AllResumeForJob(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            bool exists = Directory.Exists(Server.MapPath("~/Posted-Job-Resume/" + id + "/"));
            if (exists == false)
            {
                return HttpNotFound();
            }

            var allFileNames = Directory.GetFiles(Server.MapPath("~/Posted-Job-Resume/" + id + "/"));
            List<FileWithPath> FileWithPath = new List<FileWithPath>();
            foreach (var path in allFileNames)
            {
                FileWithPath filewithpathinstance = new FileWithPath();
                filewithpathinstance.fileUrl = path;
                filewithpathinstance.FileName = Path.GetFileName(path);
                FileWithPath.Add(filewithpathinstance);
            }
            return View(FileWithPath);
        }

        public ActionResult DownloadPostedResume(string fileName)
        {
            if (Directory.Exists(fileName))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(fileName));
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            }
            else
            {
                return HttpNotFound();
            }

        }

        // Student, instructors, admins and employers can see student activities and individual
        // profiles but student can't see other student profile.
        // This functionality will be done through nav bar.

        public ActionResult Download(string uid)
        {
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




        public ActionResult AddAttachment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAttachment(HttpPostedFileBase file)
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
                else if (fileextention == ".jpg" || fileextention == ".jpeg" || fileextention == ".bmp" || fileextention == ".png")
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

                    }

                }
            }


            return View();
        }



        // Below are extra views .    
        /// <summary>
        /// Available helpers will be shown with a way to contact then in the application
        /// </summary>
        /// <returns></returns>
        public ActionResult ResumeHelp()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ResumeHelp(HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                var fileextention = Path.GetExtension(file.FileName).ToLower();
                if (fileextention == ".pdf" || fileextention == ".docx" || fileextention == ".docm" || fileextention == ".dotx" || fileextention == ".dotm" || fileextention == ".docb" || fileextention == ".rtf")
                {
                    string uid = User.Identity.GetUserId();
                    if (uid != "")
                    {
                        string useremail = db.Users.Find(User.Identity.GetUserId()).Email;
                        if (useremail != "")
                        {
                            if (System.IO.File.Exists(Server.MapPath("~/Resume-Help/" + useremail + fileextention)))
                            {
                                // if file exist then delete it.
                                System.IO.File.Delete(Server.MapPath("~/Resume-Help/" + useremail + fileextention));
                            }
                            var path = Path.Combine(Server.MapPath("~/Resume-Help/"), useremail + fileextention);
                            file.SaveAs(path);
                        }

                    }

                }
            }


            return View();
        }

        public ActionResult AllResumeForHelp()
        {
            var allFileNames = Directory.GetFiles(Server.MapPath("~/Resume-Help/"));
            List<FileWithPath> FileWithPath = new List<FileWithPath>();
            foreach (var path in allFileNames)
            {
                FileWithPath filewithpathinstance = new FileWithPath();
                filewithpathinstance.fileUrl = path;
                filewithpathinstance.FileName = Path.GetFileName(path);
                FileWithPath.Add(filewithpathinstance);
            }
            return View(FileWithPath);
        }



        /// <summary>
        /// Create events.
        /// </summary>
        /// <returns></returns>
        public ActionResult PostEvent()
        {
            return View();
        }

        public ActionResult PostEvent([Bind(Include = "Id,EventDiscription,Date,StartTime,EndTime,Location")] Event Event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(Event);
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult AllEvents()
        {
            var events = db.Events.ToList().OrderByDescending(x => x.Date).ToList();
            if (events == null)
            {
                events = new List<Event>();
            }
            return View(events);
        }

        public ActionResult Participate(int? eventId)
        {
            if (eventId == null)
            {
                return HttpNotFound();
            }
            Event Event = db.Events.Find(eventId);
            if (Event.Date.Ticks < DateTime.Now.Ticks)
            {
                // If date is gone then can't participate.
                return HttpNotFound();
            }
            string uId = User.Identity.GetUserId();
            if (uId == "")
            {
                return HttpNotFound();
            }
            var studentProfile = db.StudentProfiles.Where(x => x.UserId == uId).FirstOrDefault();
            EventStudent eventStudent = new EventStudent();
            eventStudent.Event_Id = Event.Id;
            eventStudent.Event = Event;
            eventStudent.StudentProfile = studentProfile;
            eventStudent.Student_profileId = studentProfile.Id;
            Event.Volunteers.Add(eventStudent);
            Event.NeededVolenteers -= 1;
            db.SaveChanges();

            return RedirectToAction("AllEvents");
        }

        // Below view will only be accessible to individual student 
        /// <summary>
        /// If student has participated in any college events then employer, admin and instructor and student will be able to see that.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public ActionResult StudentParticipation(int? id)
        {
            // Based on student profile id all event participated will be added.
            StudentProfile studentProfile = db.StudentProfiles.Find(id);
            if (studentProfile == null)
            {
                return View();
            }
            var allEventsParticipatedId = db.EventParticipatedStudents.ToList().Where(x => x.Student_profileId == id).Select(x => x.Event_Id).ToList().Distinct();
            List<Event> allEventsParticipated = new List<Event>();
            foreach (var eid in allEventsParticipatedId)
            {
                Event e = db.Events.Find(eid);
                allEventsParticipated.Add(e);
            }

            return View(allEventsParticipated);
        }




        /// <summary>
        /// A list of student partial profiles who matches any skills selected or searched. This will only sear in studentprofile skills properties. Not in resume.
        /// </summary>
        /// <returns></returns>
        public ActionResult FindBySkills()
        {
            return View();
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}