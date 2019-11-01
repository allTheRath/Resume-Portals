using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume_Portal.Models
{
    public class ProgramViewModels
    {
        public string ProgramImagePath { get; set; }
        public Program program { get; set; }
    }
    public class InstructorProfileViewModels
    {
        public InstructorProfile instructorProfile { get; set; }
        public Profile profile { get; set; }
    }
    public class EmployerProfileViewModels
    {
        public EmployerProfile EmployerProfile { get; set; }
        public Profile profile { get; set; }
    }
}
