using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.Convertors;
using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Courses
{
    [PermissionChecker(11)]
    public class CreateCourseModel : PageModel
    {
        private ICourseService _courseService;
        public CreateCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [BindProperty]
        public CourseViewModel CourseViewModel { get; set; }
        public void OnGet()
        {
            CourseViewModel = _courseService.GetInformationCeraeteCourse(0);
        }
        
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CourseViewModel = _courseService.GetInformationCeraeteCourse(CourseViewModel.GroupId);
                return Page();
            }
               
            if (CourseViewModel.imgCourseUp.IsImage())
            {
                _courseService.AddCourse(CourseViewModel);
            }
            else
            {
                CourseViewModel = _courseService.GetInformationCeraeteCourse(CourseViewModel.GroupId);
                ModelState.AddModelError("UserAvatar", "لطفا یک عکس انتخاب نمایید");
            }
            return Redirect("/Admin/Courses?Succes=CreateOk");
        }
    }
}