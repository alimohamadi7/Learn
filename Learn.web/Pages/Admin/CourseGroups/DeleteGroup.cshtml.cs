using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.CourseGroups
{
    public class DeleteGroupModel : PageModel
    {
        private ICourseService _courseService;
        public DeleteGroupModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CourseGroup CourseGroups { get; set; }

        public void OnGet(int id)
        {
            CourseGroups = _courseService.GetById(id);
        }
        public IActionResult OnPost()
        {
           

            _courseService.DeleteGroup(CourseGroups);

            return Redirect("/Admin/CourseGroups?Message=DeleteOk");
        }
    }
}