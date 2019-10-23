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
                return RedirectToAction("Admin");
            }
            else if (User.IsInRole("Employer"))
            {
                return RedirectToAction("Employer");
            }
            else if (User.IsInRole("Instructor"))
            {
                return RedirectToAction("Instructor");
            }
            else if (User.IsInRole("Student"))
            {
                return RedirectToAction("Student");
            }

            return View();
        }

        // Below are the navigation bar for individual users.

        public ActionResult AdminNav()
        {
            return View();
        }

        public ActionResult EmployerNav()
        {
            return View();
        }
        public ActionResult StudentNav()
        {
            return View();
        }
        public ActionResult InstructorNav()
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

        /// <summary>
        /// Admin Home Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin()
        {
            var userDetails = db.Users.ToList().Where(x => x.Online == true).ToList();
            if (userDetails.Count() == 0)
            {
                userDetails = new List<ApplicationUser>();
            }
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

            return View(user);
        }


        /// <summary>
        /// Employer Home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Employer()
        {
            //if (!User.IsInRole("Employer"))
            //{
            //    return HttpNotFound();
            //}
            string userId = User.Identity.GetUserId();
            var employer = db.Profiles.FirstOrDefault(s => s.UserId == userId);
            //if (employer == null)
            //{
            //    return HttpNotFound();
            //}
            return View(employer);
        }

        /// <summary>
        /// Employer profile view ... Employer can edit the profile.  
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployerProfile()
        {
            //if (!User.IsInRole("Employer"))
            //{
            //    return HttpNotFound();
            //}
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
                return RedirectToAction("Index");
            }

            return View(employerProfile);
        }


        public ActionResult CreateEmployer()
        {
            //if (!User.IsInRole("Employer"))
            //{
            //    return HttpNotFound();
            //}
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
                return RedirectToAction("Index");
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




        /// <summary>
        /// Instructor home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Instructor()
        {
            //if (!User.IsInRole("Instructor"))
            //{
            //    return HttpNotFound();
            //}
            string userId = User.Identity.GetUserId();
            var instructor = db.Profiles.FirstOrDefault(s => s.UserId == userId);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        public ActionResult CreateInstructor()
        {
            //if (!User.IsInRole("Instructor"))
            //{
            //    return HttpNotFound();
            //}

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
                return RedirectToAction("Index");
            }

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
                return RedirectToAction("Index");
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
            //if (User.IsInRole("Instructor"))
            //{
            //    return HttpNotFound();
            //}
            string uid = User.Identity.GetUserId();
            InstructorProfile instructorProfile = db.InstructorProfiles.Where(x => x.UserId == uid).FirstOrDefault();
            return View(instructorProfile);
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
            //if (User.IsInRole("Student"))
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        [HttpPost, ActionName("CreateStudent")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudentConfirm([Bind(Include = "Id,AboutMe,ContactInfo,ProfetionalEmail,SemesterNumber,StartDate,EndDate,MySkills")] StudentProfile studentProfile)
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
                return RedirectToAction("Index");
            }

            return View(studentProfile);
        }



        public ActionResult EditStudent()
        {
            //if (User.IsInRole("Student"))
            //{
            //    return HttpNotFound();
            //}
            string uid = User.Identity.GetUserId();
            StudentProfile studentProfile = db.StudentProfiles.ToList().Where(x => x.UserId == uid).FirstOrDefault();
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
                return RedirectToAction("Index");
            }

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
                return RedirectToAction("Index");
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
            return View(studentProfile);
        }

        public ActionResult AddExperience()
        {
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
            return View(experiance);
        }

        [HttpPost, ActionName("DeleteExperience")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteExperienceConfirmed(int id)
        {
            Experiance experiance = db.Experiances.Find(id);
            db.Experiances.Remove(experiance);
            db.SaveChanges();
            return RedirectToAction("StudentProfile");
        }


        public ActionResult AddSkill()
        {
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
            return View(skill);
        }

        [HttpPost, ActionName("DeleteSkill")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSkillConfirmed(int id)
        {
            Skill skill = db.Skills.Find(id);
            db.Skills.Remove(skill);
            db.SaveChanges();
            return RedirectToAction("StudentProfile");
        }



        public ActionResult AddEducation()
        {
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
            return View(education);
        }

        [HttpPost, ActionName("DeleteEducation")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEducationConfirmed(int id)
        {
            Education education = db.Educations.Find(id);
            db.Educations.Remove(education);
            db.SaveChanges();
            return RedirectToAction("Index");
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
                p.Discription = p.Discription.Substring(0, 210) + "...";
            });
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
        /// All activities for a single student
        /// </summary>
        /// <returns></returns>
        public ActionResult MyActivities()
        {
            return View();
        }

        public ActionResult AddActivity()
        {
            //if(!User.IsInRole("Student"))
            //{
            //    return HttpNotFound();
            //}


            return View();
        }

        [HttpPost]
        public ActionResult AddActivity([Bind(Include = "Id,Discription,ActivityType")] Activity activity)
        {

            string uid = User.Identity.GetUserId();
            activity.ImageUrl = TempData[uid].ToString();
            activity.UserId = uid;
            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();
            }

            return View();
        }

        public ActionResult AddActivityImage()
        {
            string uid = User.Identity.GetUserId();
            string url = "";
            bool exists = Directory.Exists(Server.MapPath("~/Activity-Data/" + uid + "/"));
            if (!exists)
            {
                // if directory not exist then create it.
                Directory.CreateDirectory(Server.MapPath("~/Activity-Data/" + uid + "/"));
                url = "~/Activity-Data/" + uid + "/" + 1;
            }
            else
            {
                var allFileNames = Directory.GetFiles("~/Activity-Data/" + uid + "/");
                string fileName = allFileNames[allFileNames.Length - 1];
                var number = Path.GetFileName(fileName);
                url = "~/Activity-Data/" + uid + "/" + (Convert.ToInt32(number) + 1);
            }
            TempData[uid] = url;

            return View();
        }
        [HttpPost]
        public ActionResult AddActivityImage(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileextention = Path.GetExtension(file.FileName).ToLower();

                if (fileextention == ".jpg" || fileextention == ".jpeg" || fileextention == ".bmp" || fileextention == ".png")
                {
                    string uid = User.Identity.GetUserId();
                    if (uid != "")
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Activity-Data/" + uid + "/" + TempData[uid].ToString() + fileextention)))
                        {
                            // if file exist then delete it.
                            System.IO.File.Delete(Server.MapPath("~/Activity-Data/" + uid + "/" + TempData[uid].ToString() + fileextention));
                        }
                        var path = Path.Combine(Server.MapPath("~/Activity-Data/" + uid + "/"), TempData[uid].ToString() + fileextention);
                        file.SaveAs(path);

                    }

                }
            }
            return View();
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