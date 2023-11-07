using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Learn.web.Controllers
{
    public class CoursesController : Controller
    {
        private ICourseService _courseService;
        private IOrderService _orderService;
        private IUserService _userservice;
        private readonly IHostingEnvironment _hostingEnvironment;
        public CoursesController(ICourseService courseService, IOrderService orderService, IUserService userservice, IHostingEnvironment hostingEnvironment)
        {
            _courseService = courseService;
            _orderService = orderService;
            _userservice = userservice;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public IActionResult Index(int pageId = 1, string filter = ""
            , string getType ="all", string orderByType = "date",
            int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null)
        {
            ViewBag.ccourseCount = _courseService.CourseCount();
            ViewBag.filter = filter;
            ViewBag.selectedGroups = selectedGroups;
            ViewBag.gettype = getType;
            ViewBag.orderByType = orderByType;
            ViewBag.curentpage = pageId;
            
            ViewBag.Groups = _courseService.GetAllGroup();
            if(startPrice==0 & endPrice == 0)
            {
                ViewBag.maxprice = 2000000;
                ViewBag.minprice = 0;
            }
            else {
                ViewBag.maxprice = endPrice;
                ViewBag.minprice = startPrice;
            }
            if(!string.IsNullOrEmpty(filter))
            {
                var query = _courseService.GetCourse(pageId, filter, getType, orderByType, startPrice, endPrice, selectedGroups);
                if (query.Item2==0)
                {
                    ViewBag.msg = "برای عبارت مورد نظر شما موردی یافت نگردید.";
                    return View(_courseService.GetCourse(pageId, filter, getType, orderByType, startPrice, endPrice, selectedGroups));
                }
            }
            
            return View(_courseService.GetCourse(pageId,filter,getType,orderByType,startPrice,endPrice,selectedGroups));
        }
        [Route("ShowCourse/{id}/{*tittle}")]
        
        public IActionResult ShowCourse(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                string token = _userservice.GettokenByUsername(User.Identity.Name);

                if (token != User.FindFirst("Token").Value)
                {
                    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return Redirect("/Login?Message=OK");

                }
            }
            var course = _courseService.GetCourseForShow(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        [Authorize]
        public ActionResult BuyCourse(int id)
        {
         int orderId= _orderService.AddOrder(User.Identity.Name, id);
            return Redirect("/UserPanel/MyOrders/ShowOrder/ " + orderId);
        }
        [Route("DownloadFile/{episodeId}")]
        public IActionResult DownloadFile(int episodeId)
        {
            var episode = _courseService.GetEpisodeById(episodeId);
            var userId = _userservice.GetUserIdByUserName(User.Identity.Name);
            string filepath = Path.Combine(_hostingEnvironment.ContentRootPath, @"courseFiles\",
                episode.EpisodeFileName);
            string fileName = episode.EpisodeFileName;
            if (episode.IsFree)
            {
                _courseService.UpdateCourseForUser(episode.CourseId, userId);
                byte[] file = System.IO.File.ReadAllBytes(filepath);
                return File(file, "application/force-download", fileName,enableRangeProcessing: true);
            }

            if (User.Identity.IsAuthenticated)
            {
                if (_orderService.IsUserInCourse(User.Identity.Name, episode.CourseId))
                {
                    byte[] file = System.IO.File.ReadAllBytes(filepath);
                    return File(file, "application/force-download", fileName,enableRangeProcessing:true);
                }
            }

            return Forbid();
        }

        [HttpPost]
        [Authorize]
        public void CreateComment(string Comment ,int CourseId ,int? parentId,int? Ordercomment,int?commentId)
        {
            
            _courseService.AddComment(Comment,CourseId,parentId,_userservice.GetUserIdByUserName(User.Identity.Name), Ordercomment,commentId);

            //return View("ShowComment",_courseService.GetCourseComment(CourseId, 0, 2));
        }
       
        public IActionResult ShowComment(int courseId, int pageindex, int pagesize)
        {
            bool display = true;
           if (!User.Identity.IsAuthenticated)
            {
                 display = false;
            }
            //System.Threading.Thread.Sleep(4000);
            return Json(_courseService.GetCourseComment(courseId,pageindex,pagesize,display));
        }
       
        public IActionResult ShowCommentInCreate(int CourseId)
        {
            return Json(_courseService.GetCourseCommentForCreate(CourseId));
        }
    }
}