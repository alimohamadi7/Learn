using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Courses
{
    [PermissionChecker(18)]
    public class DeleteEpisodeModel : PageModel
    {
        private ICourseService _courseService;
        public DeleteEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }
        public void OnGet(int id,string coursename)
        {
            ViewData["CourseTitle"] = coursename;
            CourseEpisode= _courseService.GetEpisodeById(id);
        }
        public IActionResult OnPost(string coursename)
        {
          

            _courseService.DleteEpisode(CourseEpisode);
            return Redirect("/Admin/Courses/IndexEpisode/" + CourseEpisode.CourseId + "?Succes=DeleteOk&coursename=" + WebUtility.UrlEncode(coursename));

        }
    }
}