using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace Final.Models.DataAccess
{
    //TODO: This class is partially complete. Add your code to satisfy the requirements 
    public partial class Student
    {
        [NotMapped]
        [Display(Name = "Registrations")]


        public List<Course> Courses
        {
            get
            {
                List<Course> courses = new List<Course>();

                //TODO: Add your code here to satisfy the requirements
                using (RegistrationDBContext context = new RegistrationDBContext())
                {
                    courses = (from c in context.Course
                               where context.Registration.Any(r => r.CourseCourseId == c.CourseId
                                                              && r.StudentStudentNum == this.StudentNum)
                               select c).ToList<Course>();
                }



                return courses;
            }
        }
    }
}
