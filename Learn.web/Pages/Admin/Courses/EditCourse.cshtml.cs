using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Learn.web.Pages.Admin.Courses
{
    [PermissionChecker(12)]
    public class EditCourseModel : PageModel
    {
        private ICourseService _courseService;
        public EditCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [BindProperty]
        public CourseEditViewModel CourseViewModel { get; set; }
        public void OnGet(int id)
        {

            CourseViewModel = _courseService.GetInformationEditCourse(id);
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CourseViewModel = _courseService.GetInformationEditCourse(CourseViewModel.CourseId);
                return Page();
            }
                

            _courseService.UpdateCourse(CourseViewModel);

            return Redirect("/Admin/Courses?Succes=EditOk");

        }
    }
}