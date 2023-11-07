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
    [PermissionChecker(16)]
    public class CreateEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public CreateEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }



        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }
        public void OnGet(int id, string coursename)
        {
           
            CourseEpisode = new CourseEpisode();
            CourseEpisode.CourseId = id;
            ViewData["CoursePrice"] = _courseService.GetCourseById(id).CoursePrice;
            ViewData["CourseName"] = coursename;
        }
        public IActionResult OnPost(IFormFile fileEpisode, string coursename)
        {
            if (!ModelState.IsValid || fileEpisode == null)
            {
                ViewData["CourseName"] = coursename;
                return Page();
            }
                

            if (_courseService.CheckExistFile(fileEpisode.FileName))
            {
                ViewData["IsExistFile"] = true;
                ViewData["CourseName"] = coursename;
                return Page();
            }


            _courseService.AddEpisode(CourseEpisode, fileEpisode);
            

            return Redirect("/Admin/Courses/IndexEpisode/"+ CourseEpisode.CourseId+ "?Succes=CreateOk&coursename="+ WebUtility.UrlEncode(coursename));
        }
    }
}