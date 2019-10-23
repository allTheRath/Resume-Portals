﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Resume_Portal.Models
{
    public class Event
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        public string EventDiscription { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Location { get; set; }

        public int NeededVolenteers { get; set; }
        
        public ICollection<EventStudent> Volunteers { get; set; }

    }

    public class EventStudent
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        public int studentprofileId { get; set; }
        public virtual StudentProfile StudentProfile { get; set; }
    }

}