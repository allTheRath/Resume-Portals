using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resume_Portal.Models
{

    public class NotifyStudent
    {
        public int Id { get; set; }
        public int EmployerProfileId { get; set; }
        public DateTime RequestedOn { get; set; }
    }

    public class NotifyEmployer
    {
        public int Id { get; set; }
        public int StudentProfileId { get; set; }
        public bool ResumeAvailable { get; set; }
    }

    public class NotifyInstructor
    {
        public int Id { get; set; }
        public int StudentProfileId { get; set; }
        public int EmployerProfileId { get; set; }
    }

}