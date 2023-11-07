using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Courses
{
    [PermissionChecker(14)]
    public class CourseDetailsModel : PageModel
    {
        private ICourseService _courseService;
        public CourseDetailsModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public Course course { get; set; }
        public void OnGet(int id)
        {
           course= _courseService.GetCourseForDetails(id);
        }
    }
}