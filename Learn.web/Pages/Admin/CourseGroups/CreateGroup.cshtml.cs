﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.CourseGroups
{
    public class CreateGroupModel : PageModel
    {
        private ICourseService _courseService;

        public CreateGroupModel(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [BindProperty]
        public CourseGroup CourseGroups { get; set; }
        public void OnGet(int? id)
        {
            CourseGroups = new CourseGroup()
            {
                ParentId = id
            };
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _courseService.AddGroup(CourseGroups);

            return Redirect("/Admin/CourseGroups?Message=CreateOk");
        }
    }
}