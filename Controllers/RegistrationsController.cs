using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Final.Models.DataAccess;
using Final.Models;

namespace Final.Controllers
{
    
    public class RegistrationsController : Controller
    {
        public const int MaxAllowedCourses = 4;

        private readonly RegistrationDBContext _context;

        public RegistrationsController(RegistrationDBContext context)
        {
            _context = context;
        }
        public IActionResult Search()
        {
  
            return View();
        }

        //TODO: Add your code here to satisfy the requirements


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Search(string NameSearchString)
        {
            
            if (String.IsNullOrEmpty(NameSearchString) )
            {
                ModelState.AddModelError("NameSearchString","The Name Contaiins field is required.");
                return View();
            }
            if (NameSearchString.Trim().Length < 2) {
                ModelState.AddModelError("NameSearchString", "You must enter at least 2 characters!");
                return View();
            }
            if (ModelState.IsValid)
            {
                try
                {

                    List<Student> StudentSearchResults = _context.Student.Where(a => a.Name.Contains(NameSearchString.Trim())).ToList();
                    if (StudentSearchResults.Count()==0)
                    {
                        ModelState.AddModelError("NameSearchString", "No student name contains the specified characters");
                    }
                    else
                    {
                        ViewData["SearchResultList"] = StudentSearchResults;
                        return View();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            return View();
        }





        public async Task<IActionResult> CurrentRegistrations(string id) //should be the same name as being passed into
        {
            var student = await _context.Student.FirstOrDefaultAsync(a => a.StudentNum == id);
            if (student != null)
            {

                return View(student);
            }
            else {
                return NotFound();

            }
            return View();
        }





        public async Task<IActionResult> AddRegistrations(string id)
        {
            var student = await _context.Student.FirstOrDefaultAsync(a => a.StudentNum == id);
            if (student != null)
            {
                ViewData["student"] = student;
                List<CourseSelectionViewModel> courseSelectionList = new List<CourseSelectionViewModel>();
                List<Course> courseList = _context.Course.ToList();
                foreach (var course in courseList)
                {
                    Registration isRegistered = _context.Registration.ToList().Find(a => a.StudentStudentNum == student.StudentNum && a.CourseCourseId == course.CourseId);
                    if (isRegistered == null)
                    {//not registered yet
                        CourseSelectionViewModel courseCandidate = new CourseSelectionViewModel();
                        courseCandidate.TheCourse = course;
                        courseCandidate.Selected = false;
                        courseSelectionList.Add(courseCandidate);
                    }
                }
                return View(courseSelectionList);
            }
            else {//not found student
                return NotFound();
            
            }
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRegistrations(string id, List<CourseSelectionViewModel> courseList)
        {
            Student student = _context.Student.FirstOrDefault(a => a.StudentNum == id);
            //ViewBag.Student = student;
            ViewData["student"] = student;
            if (!courseList.Any(m => m.Selected))
            {
                ModelState.AddModelError("All", "You must select at least one course");
                return View(courseList);
            }
            if (ModelState.IsValid)
            {
                int selectedCount = courseList.Where(a=>a.Selected).Count();
               int registeredCount = student.Courses.Count;
                if (MaxAllowedCourses - registeredCount == 0 || selectedCount+registeredCount>MaxAllowedCourses) {
                    ModelState.AddModelError("All",$"The total number of courses exceeds the maximum allowed:{MaxAllowedCourses}");
                    return View(courseList);
                }
                

                try//add the selected courses to registration table
                {
                    foreach (CourseSelectionViewModel courseSelection in courseList)
                    {
                        if (courseSelection.Selected)
                        {

                            if (_context.Registration.Any(a => a.StudentStudentNum == student.StudentNum && a.CourseCourseId == courseSelection.TheCourse.CourseId))
                            {
                                ModelState.AddModelError("All", "The course has been registered before!");
                                return View(courseList);
                            }

                            Registration reg = new Registration();
                            //                        reg.CourseCourse = courseSelection.TheCourse;
                            reg.CourseCourseId = courseSelection.TheCourse.CourseId;
                            reg.StudentStudentNum = student.StudentNum;
                            _context.Registration.Add(reg);
                            //_context.SaveChanges();
                        }
                        _context.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction("CurrentRegistrations", new { id = id });
            }

            return View(courseList);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string CourseId, string studentNum)
        {

            Registration reg = _context.Registration.FirstOrDefault(a => a.CourseCourseId == CourseId && a.StudentStudentNum == studentNum);
            if (reg != null)
            {
                //List<Registration> registrationList = _context.Registration.ToList();
                //registrationList.Remove(reg);
                _context.Registration.Remove(reg);
                _context.SaveChanges();
            }
            else {
                return NotFound();// no record
            }
            return RedirectToAction("CurrentRegistrations", new { id = studentNum });
        }



    }
}