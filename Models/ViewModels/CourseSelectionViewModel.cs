using System;
using System.Collections.Generic;
using Final.Models.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace Final.Models
{
    //TODO: This class is partially complete. Add your code to satisfy the requirements 
    public class CourseSelectionViewModel
    {
       
        public bool Selected { get; set; } = false;
       
        public Course TheCourse { get; set; }

        public CourseSelectionViewModel() { }
        public CourseSelectionViewModel(Course course) { TheCourse = course; }
    }
}
