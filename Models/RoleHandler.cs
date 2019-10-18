using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Resume_Portal.Models
{
    public class RoleHandler
    {
        private protected UserManager<IdentityUser> userManager { get; set; }
        private protected RoleManager<IdentityRole> RoleManager { get; set; }
        private protected ApplicationDbContext db;

        public RoleHandler()
        {
            db = new ApplicationDbContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            // Initilizing manager instances.
        }

        public bool SeedDatabaseWithRoles()
        {
            RoleManager.Create(new IdentityRole("Admin"));
            RoleManager.Create(new IdentityRole("Student"));
            RoleManager.Create(new IdentityRole("Employer"));
            RoleManager.Create(new IdentityRole("Instructor"));
            return true;
        }

        public void CreateFile(string filename)
        {
            string path = @"C:\Users\jay\source\repos\Resume-Portal\ProgramsData\programdetails\" + @filename + ".txt";

            var file = File.Create(path);
            file.Close();
        }

        public void SeedDatabaseWithPrograms()
        {
            string path = @"C:\Users\jay\source\repos\Resume-Portal\ProgramsData\programNames.txt";
                
            using (StreamReader sr = File.OpenText(path))
            {
                string programName;
                while ((programName = sr.ReadLine()) != null)
                {
                    CreateFile(programName);
                }

            }

        }

        // Add online offline status to user.
        // Add Last login time to user.

        public void UpdateUserLoginStatus(string userEmail)
        {
            var userId = userManager.FindByEmail(userEmail).Id;
            if (userId != "")
            {// This If will never be false. Just to be sure.
                var user = db.Users.Find(userId);
                user.LastLogedIn = DateTime.Now;
                user.Online = true;
                db.SaveChanges();
            }
        }

        public void UpdateInvalidTry(string userEmail)
        {
            var userId = userManager.FindByEmail(userEmail).Id;
            if (userId != "")
            {// This If will never be false. Just to be sure.
                var user = db.Users.Find(userId);
                if (user != null)
                {
                    user.AccessFailedCount += 1;
                    db.SaveChanges();
                }
            }
        }

        public void UpdateUserLogOffStatus(string userId)
        {
            if (userId != "")
            {// This If will never be false. Just to be sure.
                var user = db.Users.Find(userId);
                if (user != null)
                {
                    user.Online = false;
                    db.SaveChanges();

                }
            }
        }

        public bool AssignUserToRole(string UserId, string RoleName)
        {
            var user = userManager.FindById(UserId);
            if (user == null || (!RoleManager.RoleExists(RoleName)))
            {
                return false;
            }


            bool result = userManager.AddToRole(UserId, RoleName).Succeeded;
            return result;
        }

        public bool UnassignUserToRole(string UserId, string RoleName)
        {
            var user = userManager.FindById(UserId);
            if (user == null || (!RoleManager.RoleExists(RoleName)))
            {
                return false;
            }


            bool result = userManager.RemoveFromRole(UserId, RoleName).Succeeded;
            return result;
        }

        public bool UserRequestedRegistration(string userEmail, string roleName)
        {
            var user = db.Users.Where(x => x.Email == userEmail).FirstOrDefault();
            bool result = false;
            if (userEmail == "" || user.Id == "" || roleName == "")
            {
                return result;
            }

            NotifyAdmin notifyAdmin = new NotifyAdmin();
            notifyAdmin.RequestedOn = DateTime.Now;
            notifyAdmin.Resolved = false;
            notifyAdmin.RoleName = roleName;
            notifyAdmin.UserId = user.Id;
            notifyAdmin.UserEmail = userEmail;
            db.NotifyAdmins.Add(notifyAdmin);
            db.SaveChanges();

            result = true;


            return result;

        }

        public List<string> GetAllRoles()
        {
            var result = RoleManager.Roles.ToList().Select(x => x.Name).Distinct().ToList();
            return result;
        }

    }
}