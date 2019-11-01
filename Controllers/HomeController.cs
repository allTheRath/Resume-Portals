using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Resume_Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Resume_Portal.Controllers
{
    public class HomeController : Controller
    {
        private protected ApplicationDbContext db = new ApplicationDbContext();
        private protected RoleHandler RoleHandler = new RoleHandler();
        private protected UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
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


        //public ActionResult SelectNavBar()
        //{
        //    string userId = User.Identity.GetUserId();
        //    var profile = db.Profiles.Where(p => p.UserId == userId).FirstOrDefault();
        //    if (profile.Role == "Admin")
        //    {
        //        ViewBag.Role = "Admin";
        //    }
        //    else if (profile.Role == "Student")
        //    {
        //        ViewBag.Role = "Student";
        //    }
        //    else if (profile.Role == "Instructor")
        //    {
        //        ViewBag.Role = "Instructor";
        //    }
        //    else if (profile.Role == "Employer")
        //    {
        //        ViewBag.Role = "Employer";
        //    }
        //    else
        //    {
        //        ViewBag.Role = null;
        //    }
        //    return View();
        //}

        //public ActionResult SelectProfile(string selectedUserId)
        //{
        //    string userId = User.Identity.GetUserId();
        //    if (!string.IsNullOrEmpty(selectedUserId))
        //    {
        //        userId = selectedUserId;
        //    }
        //    if (userManager.IsInRole(userId, "Student"))
        //    {
        //        var profile = db.StudentProfiles.FirstOrDefault(s => s.UserId == userId);
        //        if (profile == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return RedirectToAction("StudentDetails", "Student", new { userId = userId });
        //    }
        //    else if (userManager.IsInRole(userId, "Instructor"))
        //    {
        //        var profile = db.InstructorProfiles.FirstOrDefault(s => s.UserId == userId);
        //        if (profile == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return RedirectToAction("StudentDetails", "Student", new { userId = userId });
        //    }
        //    else if (userManager.IsInRole(userId, "Employer"))
        //    {
        //        var profile = db.EmployerProfiles.FirstOrDefault(s => s.UserId == userId);
        //        if (profile == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return RedirectToAction("EmployerDetails", "Employer", new { userId = userId });
        //    }
        //    return View();
        //}

        public ActionResult NavBar()
        {
            return View();
        }


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

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View();
        }
        //done

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

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return RedirectToAction("AllStudents", new { Id = Id });
        }
        //done
        public ActionResult SelectProgramForAccess()
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

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View();
        }
        //done Student needs to select a program only one program is available for student..

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectProgramForAccess(int Id)
        {
            string userid = User.Identity.GetUserId();

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
            ViewBag.Role = RoleHandler.GetUserRole(userid);


            var isUserExist = db.ProgramUsers.Where(x => x.UserId == userid).FirstOrDefault();
            if (isUserExist != null)
            {
                var mail = RoleHandler.GetAdminEmail();
                if (mail != null)
                {
                    ViewBag.Message = "You are already in a program. Contact " + mail + " for change.";

                }
                else
                {
                    ViewBag.Message = "You are already in a program. Contact an instructor for change.";

                }
                return View();
            }

            Program prog = db.Programs.Find(Id);
            ProgramUsers programUser = new ProgramUsers();
            programUser.UserId = userid;
            programUser.ProgramId = Id;
            db.ProgramUsers.Add(programUser);
            db.SaveChanges();
            ViewBag.Message = "You have been added to " + prog.Name;
            return View();
        }
        //done


        public ActionResult StudentProfile(int? id)
        {
            var profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            var CompleteProfile = db.StudentProfiles.Where(x => x.UserId == profile.UserId).FirstOrDefault();

            CompleteStudentInfo completeStudent = new CompleteStudentInfo();
            completeStudent.AboutMe = CompleteProfile.AboutMe;
            completeStudent.ContactInfo = CompleteProfile.ContactInfo;
            completeStudent.EndDate = CompleteProfile.EndDate;
            completeStudent.ProfileId = CompleteProfile.Id;
            completeStudent.MyName = CompleteProfile.MyName;
            completeStudent.OccupationName = CompleteProfile.OccupationName;
            completeStudent.ProfessionalEmail = CompleteProfile.ProfessionalEmail;
            completeStudent.ProfilePicUrl = profile.ProfilePic;
            completeStudent.SemesterNumber = CompleteProfile.SemesterNumber;
            completeStudent.SortDiscription = profile.ShortDiscription;
            completeStudent.StudentId = profile.UserId;

            return View(completeStudent);
        }
        //done


        public ActionResult EmployerProfile(int? id)
        {
            var profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }

            var employerProfile = db.EmployerProfiles.Where(x => x.UserId == profile.UserId).FirstOrDefault();

            EmployerProfileViewModels employerProfileViewModels = new EmployerProfileViewModels();
            employerProfileViewModels.EmployerProfile = employerProfile;
            employerProfileViewModels.profile = profile;

            return View(employerProfileViewModels);
        }
        //done

        public ActionResult MyPostedJobs(int? id)
        {
            var eid = db.Profiles.Find(id);
            var jobs = db.Jobs.ToList().Where(x => x.EmployerId == eid.UserId);
            if (jobs == null)
            {
                jobs = new List<Job>();
            }
            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View(jobs);

        }

        public ActionResult InstructorProfile(int? id)
        {
            var profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            var instructorProfile = db.InstructorProfiles.Where(x => x.UserId == profile.UserId).FirstOrDefault();

            InstructorProfileViewModels instructorProfileViewModels = new InstructorProfileViewModels();
            instructorProfileViewModels.profile = profile;
            instructorProfileViewModels.instructorProfile = instructorProfile;

            return View(instructorProfileViewModels);
        }
        //done

        //done css needed.
        public ActionResult ViewExperience(int? id)
        {
            StudentProfile user = db.StudentProfiles.Find(id);
            string userid = user.UserId;

            var experiances = db.Experiances.ToList().Where(x => x.UserId == userid).ToList();
            if (experiances == null)
            {
                experiances = new List<Experiance>();
            }

            return View(experiances);
        }
        //done

        public ActionResult ViewSkills(int? id)
        {
            StudentProfile user = db.StudentProfiles.Find(id);
            string userid = user.UserId;

            var skills = db.Skills.ToList().Where(x => x.UserId == userid).ToList();
            if (skills == null)
            {
                skills = new List<Skill>();
            }

            return View(skills);
        }
        //done

        public ActionResult ViewEducation(int? id)
        {
            StudentProfile user = db.StudentProfiles.Find(id);
            string userid = user.UserId;

            var educations = db.Educations.ToList().Where(x => x.UserId == userid).ToList();
            if (educations == null)
            {
                educations = new List<Education>();
            }

            return View(educations);
        }
        //done

        public ActionResult ViewActivities(int? id)
        {
            StudentProfile user = db.StudentProfiles.Find(id);
            string userid = user.UserId;
            var activities = db.Activities.ToList().Where(x => x.UserId == userid).ToList();
            if (activities == null)
            {
                activities = new List<Activity>();
            }

            return View(activities);
        }
        //done


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

            foreach (var i in allEmployerProfiles)
            {
                if (i.ProfilePic == null || i.ProfilePic == "")
                {
                    i.ProfilePic = "/User-Profile-Pic/blank/blankProfile.png";
                }
            }
            db.SaveChanges();
            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View(allEmployerProfiles);
        }
        //done


        /// <summary>
        ///   List of all instructors
        /// </summary>
        /// <returns></returns>
        public ActionResult AllInstructors()
        {
            var allInstructors = db.Profiles.ToList().Where(x => x.Role == "Instructor").ToList();
            string uid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(uid);
            foreach (var i in allInstructors)
            {
                if (i.ProfilePic == null || i.ProfilePic == "")
                {
                    i.ProfilePic = "/User-Profile-Pic/blank/blankProfile.png";
                }
            }
            db.SaveChanges();

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View(allInstructors);
        }
        //done


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

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View(allStudents);
        }
        //done


        /// <summary>
        /// All programs offered :: List with images and headers. A link to program discription.
        /// </summary>
        /// <returns></returns>
        public ActionResult AllPrograms()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var allPrograms = db.Programs.ToList();
            if (allPrograms.Count() == 0)
            {
                allPrograms = new List<Program>();
            }
            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View(allPrograms);
        }

        //done



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

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View(program);
        }
        //done
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

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);
            ViewBag.Students = true;
            if (allStudents.Count() == 0)
            {
                ViewBag.Students = false;
            }
            return View(allStudents);
        }
        //done

        public ActionResult AllJobs(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var profile = db.Profiles.Find(id);
            if (profile == null)
            {
                ViewBag.Content = false;
            }
            else
            {

                var jobs = db.Jobs.Where(x => x.EmployerId == profile.UserId).ToList();
                if (jobs.Count > 0)
                {
                    ViewBag.Content = true;
                }
                else
                {
                    ViewBag.Content = false;
                }
                return View(jobs);
            }

            return View();
        }
        //done

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
                job.Employer = db.EmployerProfiles.Where(x => x.UserId == job.EmployerId).FirstOrDefault();
            }

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);
            ViewBag.Jobs = true;
            if (jobs.Count() == 0)
            {
                ViewBag.Jobs = false;
            }
            return View(jobs);
        }
        //done

        //MyPostedJobs needs header and delete option done
        // Below are the navigation bar for individual users.
        //Profile Card controller code.
        public ActionResult ProfileCard()
        {
            string userId = User.Identity.GetUserId();
            var profile = db.Profiles.Where(p => p.UserId == userId).FirstOrDefault();
            if (profile == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfilePic = profile.ProfilePic;
            return View(profile);
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

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View(job);
        }
        //done


        public ActionResult RequestResume(string id)
        {
            NotifyStudent notifyStudent = new NotifyStudent();
            notifyStudent.studentId = id;
            notifyStudent.EmployerProfileId = User.Identity.GetUserId();
            notifyStudent.RequestedOn = DateTime.Now;
            notifyStudent.confirmed = false;
            db.NotifyStudents.Add(notifyStudent);
            db.SaveChanges();
            var requestedStudentProfile = db.Profiles.Where(x => x.UserId == id).FirstOrDefault();
            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View(requestedStudentProfile);
        }
        //done

        public ActionResult StudentNotifications()
        {
            string userId = User.Identity.GetUserId();
            var Employers = db.NotifyStudents.ToList().Where(x => x.studentId == userId && x.confirmed == false).ToList();
            var EmployerIds = Employers.Select(x => x.EmployerProfileId).ToList();
            var profilesOfEmployers = db.Profiles.Where(x => EmployerIds.Contains(x.UserId)).ToList();
            ViewBag.Role = "Student";

            return View(profilesOfEmployers);
        }


        public ActionResult EmployerNotifications()
        {
            string userId = User.Identity.GetUserId();
            List<NotifyEmployer> notifications = db.NotifyEmployers.Where(x => x.EmployerId == userId && x.confirmed == false).ToList();
            var StudentIds = notifications.Select(x => x.StudentProfileId).ToList();
            var IsJobResponse = notifications.Select(x => x.jobResponse).ToList();
            var profileOfStudents = db.Profiles.Where(x => StudentIds.Contains(x.UserId)).ToList();
            List<ResponseResume> ResponseResumes = new List<ResponseResume>();
            int i = 0;
            foreach (var p in profileOfStudents)
            {
                ResponseResume responseResume = new ResponseResume();
                responseResume.Profile = p;
                responseResume.JobResponse = IsJobResponse[i];
                if (IsJobResponse[i] == true)
                {
                    var n = notifications.ElementAt(i);
                    responseResume.JobId = Convert.ToInt32(n.JobId);
                }
                ResponseResumes.Add(responseResume);
                i++;
            }
            return View(ResponseResumes);
        }

        public ActionResult InstructorNotifications()
        {
            string userId = User.Identity.GetUserId();
            List<NotifyInstructor> notifications = db.NotifyInstructors.Where(x => x.InstructorId == userId && x.confirmed == false).ToList();
            List<InstructorNotifyViewModel> displayObjs = new List<InstructorNotifyViewModel>();
            foreach (var notifyIn in notifications)
            {
                InstructorNotifyViewModel instructorNotifyViewModel = new InstructorNotifyViewModel();
                instructorNotifyViewModel.Employer = db.Profiles.Where(x => x.UserId == notifyIn.EmployerProfileId).FirstOrDefault();
                instructorNotifyViewModel.Student = db.Profiles.Where(x => x.UserId == notifyIn.StudentProfileId).FirstOrDefault();

                displayObjs.Add(instructorNotifyViewModel);
            }

            return View(displayObjs);
        }

        public ActionResult ApplyToRequestForResume(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            //employer just Profile Id
            if (TempData[User.Identity.GetUserId()] != null)
            {
                TempData.Remove(User.Identity.GetUserId());
            }
            TempData.Add(User.Identity.GetUserId(), id);
            ViewBag.Role = "Student";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessResumeUpload(HttpPostedFileBase resume, HttpPostedFileBase cv, string StudentEmail)
        {
            var employerProfileId = TempData[User.Identity.GetUserId()].ToString();
            if (employerProfileId == null || StudentEmail == null)
            {
                return RedirectToAction("StudentNotifications");
            }
            string uploadedStatus = "";
            NotifyEmployer notifyEmployer = new NotifyEmployer();
            notifyEmployer.StudentProfileId = User.Identity.GetUserId();

            if (resume != null && resume.ContentLength > 0)
            {

                var fileextention = Path.GetExtension(resume.FileName).ToLower();

                if (fileextention == ".pdf" || fileextention == ".docx" || fileextention == ".docm" || fileextention == ".dotx" || fileextention == ".dotm" || fileextention == ".docb" || fileextention == ".rtf")
                {

                    bool exists = Directory.Exists(Server.MapPath("~/Employer-Requestd-Resume/" + employerProfileId + "/"));
                    if (!exists)
                    {
                        // if directory not exist then create it.
                        Directory.CreateDirectory(Server.MapPath("~/Employer-Requestd-Resume/" + employerProfileId + "/"));
                    }
                    if (System.IO.File.Exists(Server.MapPath("~/Employer-Requestd-Resume/" + employerProfileId + "/" + StudentEmail + "-resume" + fileextention)))
                    {
                        // if file exist then delete it.
                        System.IO.File.Delete(Server.MapPath("~/Employer-Requestd-Resume/" + employerProfileId + "/" + StudentEmail + "-resume" + fileextention));
                    }
                    var path = Path.Combine(Server.MapPath("~/Employer-Requestd-Resume/" + employerProfileId + "/" + StudentEmail + "-resume" + fileextention));
                    resume.SaveAs(path);
                    uploadedStatus += "Resume Uploded";
                    notifyEmployer.ResumeAvailable = true;

                }

                //Employer-Requestd-Resume
            }

            if (cv != null && cv.ContentLength > 0)
            {


                var fileextention = Path.GetExtension(cv.FileName).ToLower();
                if (fileextention == ".pdf" || fileextention == ".docx" || fileextention == ".docm" || fileextention == ".dotx" || fileextention == ".dotm" || fileextention == ".docb" || fileextention == ".rtf")
                {

                    bool exists = Directory.Exists(Server.MapPath("~/Employer-Requestd-Resume/" + employerProfileId + "/"));
                    if (!exists)
                    {
                        // if directory not exist then create it.
                        Directory.CreateDirectory(Server.MapPath("~/Employer-Requestd-Resume/" + employerProfileId + "/"));
                    }
                    if (System.IO.File.Exists(Server.MapPath("~/Employer-Requestd-Resume/" + employerProfileId + "/" + StudentEmail + "-cv" + fileextention)))
                    {
                        // if file exist then delete it.
                        System.IO.File.Delete(Server.MapPath("~/Employer-Requestd-Resume/" + employerProfileId + "/" + StudentEmail + "-cv" + fileextention));
                    }
                    var path2 = Path.Combine(Server.MapPath("~/Employer-Requestd-Resume/" + employerProfileId + "/" + StudentEmail + "-cv" + fileextention));
                    cv.SaveAs(path2);
                    uploadedStatus += "and Cover letter Uploded";
                }
                if (uploadedStatus != "")
                {


                    var employerProfile = db.Profiles.Find(Convert.ToInt32(employerProfileId));
                    uploadedStatus += "to " + employerProfile.Email;
                    notifyEmployer.EmployerId = employerProfile.UserId;
                    notifyEmployer.jobResponse = false;
                    db.NotifyEmployers.Add(notifyEmployer);
                    var instructorIds = RoleHandler.GetProgramInstuctorId(User.Identity.GetUserId());
                    foreach (var inId in instructorIds)
                    {
                        NotifyInstructor notifyInstructor = new NotifyInstructor();
                        notifyInstructor.EmployerProfileId = employerProfile.UserId;
                        notifyInstructor.InstructorId = inId;
                        notifyInstructor.StudentProfileId = User.Identity.GetUserId();
                        db.NotifyInstructors.Add(notifyInstructor);
                    }

                    db.SaveChanges();
                }
                //Employer-Requestd-Resume
            }
            ViewBag.UploadedStatus = uploadedStatus;
            ViewBag.Role = "Student";
            return View();
        }

        public ActionResult UploadResume(int? id)
        {

            if (id == null || id < 0)
            {
                return HttpNotFound();
            }
            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);
            if (TempData[userid] != null)
            {
                TempData.Remove(userid);
            }
            ViewBag.UploadedStatus = "";
            TempData.Add(userid, id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResume(HttpPostedFileBase resume, HttpPostedFileBase cv, string StudentEmail)
        {
            StringBuilder uploadedStatus = new StringBuilder();
            int jobId = Convert.ToInt32(TempData[User.Identity.GetUserId()].ToString());
            if (jobId < 0)
            {
                return RedirectToAction("StudentNotifications");
            }
            var jobApplynigFor = db.Jobs.Find(jobId);



            NotifyEmployer notifyEmployer = new NotifyEmployer();
            notifyEmployer.StudentProfileId = User.Identity.GetUserId();

            if (resume != null && resume.ContentLength > 0)
            {

                var fileextention = Path.GetExtension(resume.FileName).ToLower();

                if (fileextention == ".pdf" || fileextention == ".docx" || fileextention == ".docm" || fileextention == ".dotx" || fileextention == ".dotm" || fileextention == ".docb" || fileextention == ".rtf")
                {

                    string uid = User.Identity.GetUserId();
                    if (uid != "")
                    {
                        bool exists = Directory.Exists(Server.MapPath("~/Posted-Job-Resume/" + jobId.ToString() + "/"));
                        if (!exists)
                        {
                            // if directory not exist then create it.
                            Directory.CreateDirectory(Server.MapPath("~/Posted-Job-Resume/" + jobId.ToString() + "/"));
                        }
                        if (System.IO.File.Exists(Server.MapPath("~/Posted-Job-Resume/" + jobId.ToString() + "/" + StudentEmail + "-resume" + fileextention)))
                        {
                            // if file exist then delete it.
                            System.IO.File.Delete(Server.MapPath("~/Posted-Job-Resume/" + jobId.ToString() + "/" + StudentEmail + "-resume" + fileextention));
                        }
                        var path = Path.Combine(Server.MapPath("~/Posted-Job-Resume/" + jobId.ToString() + "/" + StudentEmail + "-resume" + fileextention));
                        resume.SaveAs(path);
                        uploadedStatus.Append("Resume Uploded ");
                        notifyEmployer.ResumeAvailable = true;

                    }

                    //Employer-Requestd-Resume
                }
            }
            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);
            ViewBag.UploadedStatus = "";

            if (cv != null && cv.ContentLength > 0)
            {


                var fileextention2 = Path.GetExtension(cv.FileName).ToLower();
                if (fileextention2 == ".pdf" || fileextention2 == ".docx" || fileextention2 == ".docm" || fileextention2 == ".dotx" || fileextention2 == ".dotm" || fileextention2 == ".docb" || fileextention2 == ".rtf")
                {

                    bool exists = Directory.Exists(Server.MapPath("~/Posted-Job-Resume/" + jobId.ToString() + "/"));
                    if (!exists)
                    {
                        // if directory not exist then create it.
                        Directory.CreateDirectory(Server.MapPath("~/Posted-Job-Resume/" + jobId.ToString() + "/"));
                    }
                    if (System.IO.File.Exists(Server.MapPath("~/Posted-Job-Resume/" + jobId.ToString() + "/" + StudentEmail + "-cv" + fileextention2)))
                    {
                        // if file exist then delete it.
                        System.IO.File.Delete(Server.MapPath("~/Posted-Job-Resume/" + jobId.ToString() + "/" + StudentEmail + "-cv" + fileextention2));
                    }
                    var path2 = Path.Combine(Server.MapPath("~/Posted-Job-Resume/" + jobId.ToString() + "/" + StudentEmail + "-cv" + fileextention2));
                    cv.SaveAs(path2);

                }
            }
            if (uploadedStatus.ToString() != "")
            {
                var eId = jobApplynigFor.EmployerId;
                var employerProfile = db.Profiles.Where(x => x.UserId == eId).FirstOrDefault();
                uploadedStatus.Append("to " + employerProfile.Email);
                notifyEmployer.EmployerId = employerProfile.UserId;
                notifyEmployer.jobResponse = true;
                notifyEmployer.JobId = jobId;
                db.NotifyEmployers.Add(notifyEmployer);
                var instructorIds = RoleHandler.GetProgramInstuctorId(User.Identity.GetUserId());
                foreach (var inId in instructorIds)
                {
                    NotifyInstructor notifyInstructor = new NotifyInstructor();
                    notifyInstructor.EmployerProfileId = employerProfile.UserId;
                    notifyInstructor.InstructorId = inId;
                    notifyInstructor.StudentProfileId = User.Identity.GetUserId();
                    db.NotifyInstructors.Add(notifyInstructor);
                }

                ViewBag.UploadedStatus = uploadedStatus.ToString();
                db.SaveChanges();
            }
            else
            {
                ViewBag.UploadedStatus = "Could not upload it.!";

            }

            ViewBag.Role = "Student";

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
            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View(FileWithPath);
        }

        public ActionResult DownloadPostedResume(string fileName)
        {
            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);


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
            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

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
            string uid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(uid);

            return View();
        }
        [HttpPost]
        public ActionResult AddAttachment(HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0)
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

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

            return View();
        }



        // Below are extra views .    
        /// <summary>
        /// Available helpers will be shown with a way to contact then in the application
        /// </summary>
        /// <returns></returns>
        public ActionResult ResumeHelp()
        {
            string uid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(uid);

            return View();
        }

        [HttpPost]
        public ActionResult ResumeHelp(HttpPostedFileBase file)
        {

            if (file != null && file.ContentLength > 0)
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

            string userid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(userid);

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
            string uid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(uid);

            return View(FileWithPath);
        }



        /// <summary>
        /// Create events.
        /// </summary>
        /// <returns></returns>
        public ActionResult PostEvent()
        {
            string uid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(uid);

            return View();
        }

        public ActionResult PostEvent([Bind(Include = "Id,EventDiscription,Date,StartTime,EndTime,Location")] Event Event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(Event);
                db.SaveChanges();
            }
            string uid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(uid);

            return View();
        }

        public ActionResult AllEvents()
        {
            var events = db.Events.ToList().OrderByDescending(x => x.Date).ToList();
            if (events == null)
            {
                events = new List<Event>();
            }
            string uid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(uid);

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
            string uid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(uid);

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
            string uid = User.Identity.GetUserId();
            ViewBag.Role = RoleHandler.GetUserRole(uid);

            return View(allEventsParticipated);
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