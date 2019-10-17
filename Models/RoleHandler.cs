using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resume_Portal.Models
{
    public class RoleHandler
    {
        private protected UserManager<IdentityUser> userManager { get; set; }
        private protected RoleManager<IdentityRole> RoleManager { get; set; }

        public RoleHandler()
        {
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            // Initilizing manager instances.
        }

        public bool AssignUserToRole(string UserId, string RoleName)
        {
            var user = userManager.FindById(UserId);
            if(user == null || (!RoleManager.RoleExists(RoleName)))
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

        

    }
}