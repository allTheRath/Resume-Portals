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
                List<ApplicationUser> applicationUsers = GenerateUsers(rand.Next(0, 5));
                foreach (var user in applicationUsers)
                {
                    userManager.Create(user, "EntityFr@mew0rk");
                    roleHandler.AssignUserToRole(user.Id, "Student");
                    ProgramUsers programUsers = new ProgramUsers { ProgramId = program.Id, UserId = user.Id };
                    db.ProgramUsers.Add(programUsers);
                }
            }
            db.SaveChanges();
        }
        public void SeedPrograms(ApplicationDbContext db)
        {
            string programNamesPath = @"~/ProgramFiles/ProgramNames.txt";
            string programDetails = @"~/ProgramFiles/ProgramDetails/";
            StringBuilder sb = null;
            using (StreamReader sr = File.OpenText(programNamesPath))
            {
                string fileName = null;
                while((fileName = sr.ReadLine()) != null)
                {
                    sb = new StringBuilder();
                    string returnedResult = File.ReadAllText(programDetails + fileName + ".txt");
                    if(!String.IsNullOrEmpty(returnedResult) || !string.IsNullOrWhiteSpace(returnedResult))
                    {
                        sb.Append(returnedResult);
                        Program program = new Program { Name = fileName, Discription = sb.ToString() };
                        db.Programs.Add(program);
                    }
                }
                db.SaveChanges();
            }
        }
        //public void SeedInstructors(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        //{

        //}
        public void SeedEmployers(UserManager<ApplicationUser> userManager)
        {
            List<ApplicationUser> applicationUsers = GenerateUsers(5);
            foreach(var user in applicationUsers)
            {
                userManager.Create(user, "EntityFr@mew0rk");
                roleHandler.AssignUserToRole(user.Id, "Employer");
            }
        }
        public void SeedSkills(ApplicationDbContext db)
        {
            Skill skill0 = new Skill { SkillName = "Organized" };
            Skill skill1 = new Skill { SkillName = "Proficent" };
            Skill skill2 = new Skill { SkillName = "Multi-tasker" };
            Skill skill3 = new Skill { SkillName = "Punctual" };
            Skill skill4 = new Skill { SkillName = "Leadership" };
            Skill skill5 = new Skill { SkillName = "Fast learner" };
            Skill skill6 = new Skill { SkillName = "Teamwork" };
            Skill skill7 = new Skill { SkillName = "Communicative" };
            db.Skills.Add(skill0);
            db.Skills.Add(skill1);
            db.Skills.Add(skill2);
            db.Skills.Add(skill3);
            db.Skills.Add(skill4);
            db.Skills.Add(skill5);
            db.Skills.Add(skill6);
            db.Skills.Add(skill7);
        }
        public List<ApplicationUser> GenerateUsers(int howManyUsers)
        {
            List<ApplicationUser> applicationUsers = new List<ApplicationUser>();
            
            string[] names = { "Joe", "Mary", "Ethan", "Mister", "Bob", "Elizabeth", "Tiffany", "Mark", "Reggie", "Jay", "Keegan", "Jeff", "Natashia", "Steve", "Rogers", "Thor", "Billy" };
            for (int i = 0; i < howManyUsers; i++)
            {
                int nameIndex = rand.Next(0, names.Length - 1);
                ApplicationUser user = new ApplicationUser { UserName = names[nameIndex] + "@test.com", Email = names[nameIndex] + "@test.com", LastLogedIn = DateTime.Now };
                applicationUsers.Add(user);
            }
            return applicationUsers;
        }
    }
}