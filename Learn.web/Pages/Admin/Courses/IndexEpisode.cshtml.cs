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
    [PermissionChecker(15)]
    public class IndexEpisodeModel : PageModel
    {
        private ICourseService _courseService;

        public IndexEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        public List <CourseEpisode> CourseEpisode { get; set; }
        public void OnGet(int id,string Succes,string coursename)
        {
            CourseEpisode= _courseService.GetListEpisodeCorse(id);
            ViewData["Message"] = Succes;
            ViewData["CourseId"] = id;
            ViewData["CourseTitle"] = coursename;
        }
    }
}