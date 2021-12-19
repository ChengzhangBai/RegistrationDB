using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Final.Models.DataAccess;


namespace Final.Models
{
    //TODO: This class is partially complete. Add your code to satisfy the requirements 
    public class SearchViewModel
    {

        [Required]
        [MinLength(2, ErrorMessage = "You must enter at least 2 characters!")]
        [Display(Name = "Name Contains:")]
        public string NameSearchString { get; set; }

        public List<Student> StudentSearchResults { get; set; } = new List<Student>();

    }
    
}
