using System.ComponentModel.DataAnnotations;


namespace Final.Models.DataAccess
{
    public class StudentMetaData
    {
        [Display(Name ="Student Number")]
        public string StudentNum { get; set; }

        [Display(Name="Student Name")]
        public string Name { get; set; }
    }
}
