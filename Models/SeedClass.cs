using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;
using System.Text;

namespace Resume_Portal.Models
{
    public class SeedClass
    {
        private Random rand = new Random();
        private RoleHandler roleHandler = new RoleHandler();
        public void SeedStudents(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            var programs = db.Programs.ToList();
            foreach (var program in programs)
            {
                int userCount = db.Users.Count();
                List<ApplicationUser> applicationUsers = GenerateUsers(rand.Next(1, 5), userCount);
                foreach (var user in applicationUsers)
                {
                    userManager.Create(user, "EntityFr@mew0rk");
                    roleHandler.AssignUserToRole(user.Id, "Student");
                    ProgramUsers programUsers = new ProgramUsers() { ProgramId = program.Id, UserId = user.Id };
                    db.ProgramUsers.Add(programUsers);
                    db.SaveChanges();
                }
            }

        }
        public void SeedPrograms(ApplicationDbContext db)
        {
            string programNamesPath = @"ProgramFiles\ProgramNames.txt";
            string programDetails = @"ProgramFiles\ProgramDetails\";
            StringBuilder sb = new StringBuilder();
            using (StreamReader sr = File.OpenText(programNamesPath))
            {
                string fileName = null;
                while ((fileName = sr.ReadLine()) != null)
                {
                    if (File.Exists(programDetails + fileName + ".txt"))
                    {
                        string returnedResult = File.ReadAllText(programDetails + fileName + ".txt");
                        if (!String.IsNullOrEmpty(returnedResult) || !string.IsNullOrWhiteSpace(returnedResult))
                        {
                            sb.Append(returnedResult);
                            Program program = new Program { Name = fileName, Discription = sb.ToString() };
                            db.Programs.Add(program);
                        }

                    }
                }
                db.SaveChanges();
            }
        }
        //public void SeedInstructors()
        //{

        //}
        //public void SeedEmployers()
        //{

        //}

        public List<ApplicationUser> GenerateUsers(int howManyUsers, int userCounter)
        {
            List<ApplicationUser> applicationUsers = new List<ApplicationUser>();

            string[] names = new string[] { "Joe", "Mary", "Ethan", "Mister", "Bob", "Elizabeth", "Tiffany", "Mark", "Reggie", "Jay", "Keegan", "Jeff", "Natashia", "Steve", "Rogers", "Thor", "Billy" };
            for (int i = 0; i < howManyUsers; i++)
            {
                int nameIndex = rand.Next(0, names.Length - 1);
                ApplicationUser user = new ApplicationUser { UserName = names[nameIndex] + "@test.com", Email = names[nameIndex] + "@test" + userCounter++ + " .com", LastLogedIn = DateTime.Now };
                applicationUsers.Add(user);
            }
            return applicationUsers;
        }
    }
}