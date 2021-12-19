using System;
using System.Collections.Generic;

namespace Final.Models.DataAccess
{
    public partial class Course
    {
        public Course()
        {
            Registration = new HashSet<Registration>();
        }

        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string Description { get; set; }
        public int? HoursPerWeek { get; set; }
        public decimal? FeeBase { get; set; }

        public virtual ICollection<Registration> Registration { get; set; }
    }
}
