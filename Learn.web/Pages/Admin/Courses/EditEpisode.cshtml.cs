using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Courses
{
    [PermissionChecker(17)]
    public class EditEpisodeModel : PageModel
    {
        private ICourseService _courseService;
        public EditEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }
        public void OnGet(int id ,string coursename)
        {
            CourseEpisode= _courseService.GetEpisodeById(id);
            ViewData["CourseName"]= coursename;
        }
        public IActionResult OnPost(string coursename, IFormFile fileEpisode)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CourseName"] = coursename;
                return Page();
            }
        

            if (fileEpisode != null)
            {
                if (_courseService.CheckExistFile(fileEpisode.FileName))
                {
                    ViewData["IsExistFile"] = true;
                    return Page();
                }
            }


            _courseService.EditEpisode(CourseEpisode, fileEpisode);

            return Redirect("/Admin/Courses/IndexEpisode/" + CourseEpisode.CourseId + "?Succes=EditOk&coursename=" + WebUtility.UrlEncode(coursename));
        }
    }
}