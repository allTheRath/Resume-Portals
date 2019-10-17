using Resume_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Resume_Portal.Controllers
{
    public class HomeController : Controller
    {
        private protected ApplicationDbContext db = new ApplicationDbContext();

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


        /// <summary>
        /// Admin Home Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin()
        {
            return View();
        }


        /// <summary>
        /// Admin profile view.
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminProfile()
        {
            return View();
        }

        // Admin and instructor can assign students and employers.
        /// <summary>
        ///  Multiple users can be assigned to appropriate roles. Filter user by requested roles roles
        /// </summary>
        /// <returns></returns>
        public ActionResult AssignUsers()
        {
            return View();
        }

        /// <summary>
        ///  Multiple users can be un assigned to a role.
        /// </summary>
        /// <returns></returns>
        public ActionResult UnAssignUsers()
        {
            return View();
        }


        /// <summary>
        /// Employer Home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Employer()
        {
            return View();
        }
        // Employer can see list of students or can see program home page.
        // Employer can send email to any student or instructor.
        // Student can see notification of sent email.

        public ActionResult CreateEmployer()
        {
            return View();
        }

        [HttpPost, ActionName("CreateEmployer")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployerConfirmation([Bind(Include = "Id,AboutUs,HistoryOfCompany,ContactUs,PhoneNo,LookingForSkills")] EmployerProfile employerProfile)
        {
            if (ModelState.IsValid)
            {
                db.EmployerProfiles.Add(employerProfile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employerProfile);
        }


        /// <summary>
        /// Employer can post job to a program. All students of that program will be able to see the job.
        /// </summary>
        /// <returns></returns>
        public ActionResult PostJob()
        {
            return View();
        }

        /// <summary>
        /// Employer profile view ... Employer can edit the profile.  
        /// </summary>
        /// <returns></returns>
        public ActionResult EmployerProfile()
        {
            return View();
        }



        /// <summary>
        /// Instructor home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Instructor()
        {
            return View();
        }

        public ActionResult CreateInstructor()
        {
            return View();
        }

        [HttpPost, ActionName("CreateInstructor")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateInstructorConfirm([Bind(Include = "Id,AboutMe,Experience,JoinedMitt,ContactInfo,ProfetionalEmail")] InstructorProfile instructorProfile)
        {

            if (ModelState.IsValid)
            {
                db.InstructorProfiles.Add(instructorProfile);
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
            return View();
        }


        /// <summary>
        /// Student Home page
        /// </summary>
        /// <returns></returns>
        public ActionResult Student()
        {
            return View();
        }
        // Can add activities
        // Can update resume or profile
        // Can See all employers
        // Can see program home. :: which will have, program discription, all students of program and events 
        // Can't see other student profiles. Only partial profile will be visible in program home page.

        public ActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost, ActionName("CreateStudent")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudentConfirm([Bind(Include = "Id,AboutMe,ContactInfo,ProfetionalEmail,SemesterNumber,StartDate,EndDate,MySkills")] StudentProfile studentProfile)
        {
            if (ModelState.IsValid)
            {
                db.StudentProfiles.Add(studentProfile);
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
            return View();
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
            return View();
        }

        /// <summary>
        ///   List of all instructors
        /// </summary>
        /// <returns></returns>
        public ActionResult AllInstructors()
        {
            return View();
        }

        /// <summary>
        /// All students in College ,Seperated by programs.
        /// </summary>
        /// <returns></returns>
        public ActionResult AllStudents()
        {
            return View();
        }

        /// <summary>
        /// All programs offered :: List with images and headers. A link to program discription.
        /// </summary>
        /// <returns></returns>
        public ActionResult AllPrograms()
        {
            return View();
        }

        /// <summary>
        /// Program discription based on program id // A below view will be list of users/Instructors/Students/Employers in this program 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProgramDetails(int? id)
        {
            return View();
        }

        /// <summary>
        /// Student list by program id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult StudentInProgram(int? id)
        {
            return View();
        }

        // Student, instructors, admins and employers can see student activities and individual
        // profiles but student can't see other student profile.
        // This functionality will be done through nav bar.


        // Below are extra views .    
        /// <summary>
        /// Available helpers will be shown with a way to contact then in the application
        /// </summary>
        /// <returns></returns>
        public ActionResult ResumeHelp()
        {
            return View();
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
            return View();
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