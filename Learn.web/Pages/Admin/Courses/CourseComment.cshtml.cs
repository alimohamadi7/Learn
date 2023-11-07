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

namespace Learn.web.Pages.Admin.Courses
{
    [PermissionChecker(23)]
    public class CourseCommentModel : PageModel
    {
        private ICourseService _courseService;
        public CourseCommentModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        
        public CourseCommentForAdminViweModel CourseComment { get; set; }
        public void OnGet(int pageId=1 ,string trim="",bool checkajax=false)
        {
            CourseComment = _courseService.GetCommentUnRead(pageId,trim,checkajax);
            if (pageId - 1 > CourseComment.PageCount)
            {
                ViewData["NotPage"] = "این صفحه وجود ندارد";
            }
            
        }
        
        public IActionResult OnPostEditComment(int CommentId, string Comment)
        {
         return Content(  _courseService.UpdateComment(CommentId, Comment));
            
        }
        public IActionResult OnPostReadComment(List<int> CommentId)
        {
            return Content(_courseService.ReadComment(CommentId).ToString());
        }
        public IActionResult OnPostDeleteComment(int CommentId)
        {
            return Content(_courseService.DeleteComment(CommentId).ToString());
        }
    }
}