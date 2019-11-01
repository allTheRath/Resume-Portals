using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Resume_Portal.Models
{

    public class NotifyStudent
    {
        public int Id { get; set; }
        public string EmployerProfileId { get; set; }
        public string studentId { get; set; }
        public DateTime RequestedOn { get; set; }
        public bool confirmed { get; set; }
    }

    public class NotifyEmployer
    {
        public int Id { get; set; }
        public string StudentProfileId { get; set; }
        public string EmployerId { get; set; }
        public bool ResumeAvailable { get; set; }
        public bool confirmed { get; set; }
        public bool jobResponse { get; set; }
        public int? JobId { get; set; }
    }

    public class NotifyInstructor
    {
        public int Id { get; set; }
        public string StudentProfileId { get; set; }
        public string EmployerProfileId { get; set; }
        public string InstructorId { get; set; }
        public bool confirmed { get; set; }

    }

    public class ResponseResume
    {
        public Profile Profile { get; set; }
        public bool JobResponse { get; set; }
        public int JobId { get; set; }
    }

    public class InstructorNotifyViewModel
    {
        public Profile Student { get; set; }
        public Profile Employer { get; set; }
    }
}