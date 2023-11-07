using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Courses
{
    [PermissionChecker(10)]

    public class IndexModel : PageModel
    {
        private ICourseService _courseService;
        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public CourseForIndexViweModel courseForIndexViweModel { get; set; }
        public void OnGet(int PageId =1, string trim ="", string Succes ="")
        {
            courseForIndexViweModel= _courseService.GetCoursesForAdmin(PageId,trim,Succes);
            if (PageId-1 > courseForIndexViweModel.PageCount)
            {
                ViewData["NotPage"] = "این صفحه وجود ندارد";
            }
        }
    }
}