using System.ComponentModel.DataAnnotations;

namespace Final.Models.DataAccess
{
    public class CourseMetaData
    {
        [Display(Name = "Course Code")]
        public string CourseId { get; set; }

        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }

        [Display(Name = "Course Description")]
        public string Description { get; set; }

        [Display(Name = "Hours/Week")]
        public int? HoursPerWeek { get; set; }

        [Display(Name = "Fee Base")]
        public decimal? FeeBase { get; set; }

    }
}
