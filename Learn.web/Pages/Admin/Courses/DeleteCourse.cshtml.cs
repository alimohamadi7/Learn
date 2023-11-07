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
    [PermissionChecker(13)]
    public class DeleteCourseModel : PageModel
    {
        private ICourseService _courseService;
        public DeleteCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [BindProperty]
        public  Course Course { get; set; }
        public void OnGet(int id)
        {
            Course = _courseService.GetCourseById(id);
        }
        public IActionResult OnPost()
        {
            _courseService.DeleteCorse(Course);
            return Redirect("/Admin/Courses?Succes=DeleteOk");
        }
    }
}