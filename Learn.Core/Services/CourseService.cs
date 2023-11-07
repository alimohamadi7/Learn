using Learn.Core.Convertors;
using Learn.Core.DTOs;
using Learn.Core.Generator;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Context;
using Learn.DataLayer.Entities.Course;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vereyon.Web;
using System.Threading.Tasks;

namespace Learn.Core.Services
{
    public class CourseService : ICourseService
    {
        private LearnContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IPermissionService _permissionService;
        public CourseService(LearnContext context ,IHostingEnvironment hostingEnvironment, IPermissionService permissionService)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _permissionService = permissionService;
           
        }
        public List<CourseGroup> GetAllGroup()
        {
            return _context.CourseGroups.Include(g=>g.CourseGroups).ToList();
        }

        public List<SelectListItem> GetGroupForManageCourse()
        {
            return _context.CourseGroups.Where(g => g.ParentId == null).Select(g => new SelectListItem()
            {
                Text = g.GroupTitle,
                Value = g.GroupId.ToString()
            }
                 ).ToList();

        }
        public List<SelectListItem> GetSubGroupForManageCourse(int? groupId)
        {
            return _context.CourseGroups.Where(g => g.ParentId == groupId)
               .Select(g => new SelectListItem()
               {
                   Text = g.GroupTitle,
                   Value = g.GroupId.ToString()
               }).ToList();
        }
        public CourseViewModel GetInformationCeraeteCourse(int? id)
        {
            var groups = GetGroupForManageCourse();
            var subGrous = GetSubGroupForManageCourse(id);
            var teachers = GetTeachers();
            var levels = GetLevels();
            var statues = GetStatues();
            CourseViewModel courseViewModel = new CourseViewModel()
            {
                Groups = new SelectList(groups, "Value", "Text"),
                SupGroups = new SelectList(subGrous, "Value", "Text"),
                Teachers = new SelectList(teachers, "Value", "Text"),
                Levels = new SelectList(levels, "Value", "Text"),
                Stautus = new SelectList(statues, "Value", "Text")
            };
            return courseViewModel;
        }

        public List<SelectListItem> GetLevels()
        {
            return _context.CourseLevels.Select(l => new SelectListItem()
            {
                Value = l.LevelId.ToString(),
                Text = l.LevelTitle
            }).ToList();
        }

        public List<SelectListItem> GetStatues()
        {
            return _context.CourseStatuses.Select(s => new SelectListItem()
            {
                Value = s.StatusId.ToString(),
                Text = s.StatusTitle
            }).ToList();

        }



        public List<SelectListItem> GetTeachers()
        {
            return _context.UserRoles.Where(r => r.RoleId == 2).Include(r => r.User)
               .Select(u => new SelectListItem()
               {
                   Value = u.UserId.ToString(),
                   Text = u.User.Name + " " + u.User.LastName
               }).ToList();
        }

        public CourseForIndexViweModel GetCoursesForAdmin(int PageId = 1, string trim = "", string Succes = "")
        {
            IQueryable<Course> result = _context.Courses.Include(e => e.CourseEpisodes).AsNoTracking();
            if (!String.IsNullOrEmpty(trim))
            {
                result = result.Where(c => c.CourseTitle.Contains(trim));
            }
            //Show Item In Page
            int take = 10;
            int skip = (PageId - 1) * take;
            CourseForIndexViweModel Course = new CourseForIndexViweModel()
            {
                CurrentPage = PageId,
                PageCount = (int)Math.Ceiling(result.Count() / (double)take),
                trim = trim,
                Message = Succes,
                Courses = result.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList()

            };
            return Course;
        }

        public int AddCourse(CourseViewModel course)
        {
            
            Course course1 = new Course();
            course1.CreateDate = DateTime.Now;
            course1.CourseImageName = "Course.png";
            course1.CourseTitle = course.CourseTitle;
            course1.GroupId = course.GroupId;
            course1.SubGroup = course.SubGroup;
            course1.CoursePrice = course.CoursePrice;
            course1.CourseDescription = course.CourseDescription;
            course1.Tags = course.Tags;
            course1.TeacherId = course.TeacherId;
            course1.LevelId = course.LevelId;
            course1.StatusId = course.StatusId;
            course1.ShowComment = course.ShowComent;
            course1.CourseShortDescription = course.CourseShortDescription;
            course1.CourseImageName = GenerateUniq.GenerateUniqCode() + Path.GetExtension(course.imgCourseUp.FileName);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/image", course1.CourseImageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                course.imgCourseUp.CopyTo(stream);
            }

            ImageConvertor imgResizer = new ImageConvertor();
            string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/thumb", course1.CourseImageName);

            imgResizer.Image_resize(imagePath, thumbPath, 280);


            if (course.demoUp != null)
            {
                course1.DemoFileName = GenerateUniq.GenerateUniqCode() + Path.GetExtension(course.demoUp.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/demoes", course1.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    course.demoUp.CopyTo(stream);
                }
            }

            _context.Courses.Add(course1);
            _context.SaveChanges();

            return course.CourseId;

        }

        public Course GetCourseById(int courseId)
        {
            return _context.Courses.Find(courseId);
        }

        public CourseEditViewModel GetInformationEditCourse(int id)
        {
            Course course = GetCourseById(id);
            var groups = GetGroupForManageCourse();
            var subGrous = GetSubGroupForManageCourse(course.GroupId);
            var teachers = GetTeachers();
            var levels = GetLevels();
            var statues = GetStatues();
            CourseEditViewModel courseViewModel = new CourseEditViewModel()
            {

                Groups = new SelectList(groups, "Value", "Text", course.GroupId),
                SupGroups = new SelectList(subGrous, "Value", "Text", course.SubGroup ?? 0),
                Teachers = new SelectList(teachers, "Value", "Text", course.TeacherId),
                Levels = new SelectList(levels, "Value", "Text", course.LevelId),
                Stautus = new SelectList(statues, "Value", "Text", course.StatusId),
                CourseImageName = course.CourseImageName,
                CourseTitle = course.CourseTitle,
                CourseDescription = course.CourseDescription,
                CourseShortDescription=course.CourseShortDescription,
                CoursePrice = course.CoursePrice,
                CourseId = course.CourseId,
                Tags = course.Tags,
                DemoFileName = course.DemoFileName,
                GroupId = course.GroupId,
                ShowComent=course.ShowComment
            };
            return courseViewModel;
        }

        public void UpdateCourse(CourseEditViewModel CourseViewModel)
        {
            Course course = _context.Courses.Find(CourseViewModel.CourseId);
            if (CourseViewModel.imgCourseUpEdit != null && CourseViewModel.imgCourseUpEdit.IsImage())
            {
                if (course.CourseImageName != "Course.png")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/image", course.CourseImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/thumb", course.CourseImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                    CourseViewModel.CourseImageName = GenerateUniq.GenerateUniqCode() + Path.GetExtension(CourseViewModel.imgCourseUpEdit.FileName);
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/image", CourseViewModel.CourseImageName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        CourseViewModel.imgCourseUpEdit.CopyTo(stream);
                    }

                    ImageConvertor imgResizer = new ImageConvertor();
                    string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/thumb", CourseViewModel.CourseImageName);

                    imgResizer.Image_resize(imagePath, thumbPath, 280);
                }
            }
            if (CourseViewModel.demoUp != null)
            {
                if (course.DemoFileName != null)
                {
                    string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/demoes", course.DemoFileName);
                    if (File.Exists(deleteDemoPath))
                    {
                        File.Delete(deleteDemoPath);
                    }
                }
                CourseViewModel.DemoFileName = GenerateUniq.GenerateUniqCode() + Path.GetExtension(CourseViewModel.demoUp.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/demoes", CourseViewModel.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    CourseViewModel.demoUp.CopyTo(stream);
                }
            }
            //delete demo file and demomo file empty
            if (CourseViewModel.demoDelete == true)
            {
                string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/demoes", course.DemoFileName);
                if (File.Exists(deleteDemoPath))
                {
                    File.Delete(deleteDemoPath);
                }
                CourseViewModel.DemoFileName = null;
            }
            
            course.CourseImageName = CourseViewModel.CourseImageName;
            course.DemoFileName = CourseViewModel.DemoFileName;
            course.CourseTitle = CourseViewModel.CourseTitle;
            course.CourseDescription = CourseViewModel.CourseDescription;
            course.CoursePrice = CourseViewModel.CoursePrice;
            course.CourseId = CourseViewModel.CourseId;
            course.Tags = CourseViewModel.Tags;
            course.GroupId = CourseViewModel.GroupId;
            course.LevelId = CourseViewModel.LevelId;
            course.SubGroup = CourseViewModel.SubGroup;
            course.TeacherId = CourseViewModel.TeacherId;
            course.StatusId = CourseViewModel.StatusId;
            course.ShowComment = CourseViewModel.ShowComent;
            course.CourseShortDescription =CourseViewModel.CourseShortDescription;

            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public void DeleteCorse(Course course)
        {
            if (course.CourseImageName != "Course.png")
            {
                string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/image", course.CourseImageName);
                if (File.Exists(deleteimagePath))
                {
                    File.Delete(deleteimagePath);
                }

                string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/thumb", course.CourseImageName);
                if (File.Exists(deletethumbPath))
                {
                    File.Delete(deletethumbPath);
                }
                if (course.DemoFileName != null)
                {
                    string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseImage/demoes", course.DemoFileName);
                    if (File.Exists(deleteDemoPath))
                    {
                        File.Delete(deleteDemoPath);
                    }
                }
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }

        public Course GetCourseForDetails(int id)
        {
            return _context.Courses.Include(c=>c.CourseGroup).Where(c=>c.CourseId==id)
                .Include(g=>g.Group)
                .Include(l=>l.CourseLevel).Include(s=>s.CourseStatus).Single();
        }

        public List<CourseEpisode> GetListEpisodeCorse(int courseId)
        {
            return _context.CourseEpisodes.Where(e => e.CourseId == courseId).ToList();
        }

        public bool CheckExistFile(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles", fileName);
            return File.Exists(path);
        }

        public int AddEpisode(CourseEpisode episode, IFormFile episodeFile)
        {

            
            episode.EpisodeFileName = episodeFile.FileName;

            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, @"courseFiles\", episode.EpisodeFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                episodeFile.CopyTo(stream);
            }

            _context.CourseEpisodes.Add(episode);
            CourseUpdateDate(episode.CourseId);
            _context.SaveChanges();
            
            return episode.EpisodeId;
        }

        public CourseEpisode GetEpisodeById(int episodeId)
        {
            return _context.CourseEpisodes.Find(episodeId);
        }

        public void EditEpisode(CourseEpisode episode, IFormFile episodeFile)
        {
            if (episodeFile != null)
            {
                string deleteFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, @"courseFiles\", episode.EpisodeFileName);
                File.Delete(deleteFilePath);

                episode.EpisodeFileName = episodeFile.FileName;
                string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, @"courseFiles\", episode.EpisodeFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    episodeFile.CopyTo(stream);
                }
            }

            _context.CourseEpisodes.Update(episode);
            _context.SaveChanges();
        }

        public void DleteEpisode(CourseEpisode courseEpisode)
        {
           
            if (courseEpisode.EpisodeFileName != null)
            {
                string path= Path.Combine(_hostingEnvironment.ContentRootPath, @"courseFiles\", courseEpisode.EpisodeFileName);
                File.Delete(path);
            }
            _context.CourseEpisodes.Remove(courseEpisode);
            _context.SaveChanges();
        }

        public Tuple< List <ShowCourseListItemForArchiveViewModel>, int> GetCourse(int pageId = 1, string filter = ""
               , string getType = "all", string orderByType = "date",
               int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null, int take = 0)
        {
            if (take == 0)
                take = 1;

            IQueryable<Course> result = _context.Courses.AsNoTracking();

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.CourseTitle.Contains(filter) || c.Tags.Contains(filter));
              
            }
       
            switch (getType)
            {
                case "all":
                    break;
                case "buy":
                    {
                        result = result.Where(c => c.CoursePrice != 0);
                        break;
                    }
                case "free":
                    {
                        result = result.Where(c => c.CoursePrice == 0);
                        break;
                    }

            }

            switch (orderByType)
            {
                case "date":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                case "updatedate":
                    {
                        result = result.OrderByDescending(c => c.UpdateDate);
                        break;
                    }
                case "price":
                    {
                        result = result.OrderByDescending(c => c.CoursePrice);
                        break;
                    }

            }

            if (startPrice > 0)
            {
                result = result.Where(c => c.CoursePrice > startPrice);
            }

            if (endPrice > 0)
            {
                result = result.Where(c => c.CoursePrice < endPrice);
            }


            if (selectedGroups != null && selectedGroups.Any())
            {

                result = result.Where(c => selectedGroups.Contains(c.GroupId)|| selectedGroups.Contains(c.SubGroup.Value));

                //foreach (int groupId in selectedGroups)
                //{

                //    result = result.Where(c => c.GroupId == groupId|| c.SubGroup == groupId);

                //}
            }

            int skip = (pageId - 1) * take;
            int pagecount = (int)Math.Ceiling(result.Count() / (double)take);
            var query= result.Include(c => c.CourseEpisodes).Select(c => new ShowCourseListItemForArchiveViewModel()
            {
               
                    CourseId = c.CourseId,
                    ImageName = c.CourseImageName,
                    Price = c.CoursePrice,
                    Title = c.CourseTitle,
                    CreateDate = c.CreateDate,
                    TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
            }).Skip(skip).Take(take).ToList();
            return Tuple.Create(query, pagecount);
        }

        public List<ShowCourseListItemViewModel> GetLastcourses()
        {
            IQueryable<Course> result = _context.Courses;
            
            return result.Include(c => c.CourseEpisodes).Select(c => new ShowCourseListItemViewModel()
            {
                CourseId = c.CourseId,
                ImageName = c.CourseImageName,
                Price = c.CoursePrice,
                Title = c.CourseTitle,
                CreateDate=c.CreateDate,
                TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
            }).Take(8).OrderByDescending(c=>c.CreateDate).ToList();
        }

        public Course GetCourseForShow(int courseId)
        {
            return _context.Courses.Include(c => c.CourseEpisodes)
                .Include(c => c.CourseStatus).Include(c => c.CourseLevel)
                .Include(c => c.User).Include(uc=>uc.UserCourses)
                .FirstOrDefault(c => c.CourseId == courseId);
        }

        public int CourseCount()
        {
            return _context.Courses.Count();
        }

        public void CourseUpdateDate(int coursId)
        {
            Course course = GetCourseById(coursId);
            course.UpdateDate = DateTime.Now;
            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public void UpdateCourseForUser(int CourseId, int userId)
        {
            bool courseuser = _context.UserCourses.Any(u=>u.CourseId==CourseId && u.UserId==userId);
            if (courseuser == false)
            {
                _context.UserCourses.Add(new UserCourse()
                {
                    CourseId = CourseId,
                    UserId = userId
                });
                _context.SaveChanges();
            }
         
        }
        public void AddComment(string Comment, int courseId, int? parentId, int userId,int? Ordercomment,int?commentId)
        {
            bool role = _permissionService.GetRoleByUsrId(userId);
            var sanitizer = HtmlSanitizer.SimpleHtml5DocumentSanitizer();
            CourseComment comment = new CourseComment();
            comment.Comment = sanitizer.Sanitize(Comment); 
            comment.CourseId = courseId;
            comment.ParentId = parentId;
            comment.CreateDate = DateTime.Now;
            comment.UserId = userId;
            
            if(parentId != null)
            {
                comment.OrderComment = parentId;
                if (commentId != null)
                {
                    var com = _context.CourseComments.Find(commentId);
                    com.IsAdminRead = true;
                    _context.CourseComments.Update(com);
                    _context.SaveChanges();
                }
            }

            if (role==true)
            {
                comment.IsAdminRead = true;
            }
            _context.CourseComments.Add(comment);
            _context.SaveChanges();
            if (parentId == null)
            {
                comment.OrderComment = _context.CourseComments.Max(c => c.CommentId);
                _context.CourseComments.Update(comment);
                _context.SaveChanges();
            }
        }

        List<CourseCommentViewModel> ICourseService.GetCourseComment(int courseId, int pageindex, int pagesize, bool display)
        {

            return _context.CourseComments.Include(c => c.User).Where(c => c.CourseId == courseId&&c.IsAdminRead==true).OrderBy(p => p.OrderComment).Skip(pageindex * pagesize).Take(pagesize)
                .Select(c => new CourseCommentViewModel()
                   {
                       Comment = c.Comment,
                       CommentId = c.CommentId,
                       CreateDate = c.CreateDate.ToLongPersianDateTimeString(),
                       ParentId = c.ParentId,
                       UserAvatar = c.User.UserAvatar,
                       UserName = c.User.UserName,
                       Display=display
                   }).ToList();


        }

        public List<CourseCommentViewModel> GetCourseCommentForCreate(int CourseId)
        {


           return _context.CourseComments.Include(c => c.User).OrderBy(c=>c.OrderComment).Where(c => c.CourseId == CourseId&&c.IsAdminRead==true).Take(5)
                  .Select(c => new CourseCommentViewModel()
                  {
                      CommentId = c.CommentId,
                      Comment = c.Comment,
                      CreateDate = c.CreateDate.ToLongPersianDateTimeString(),
                      ParentId = c.ParentId,
                      UserAvatar = c.User.UserAvatar,
                      UserName = c.User.UserName
                  }).ToList();
        }

        public int GetCommentCount(int CourseId)
        {
          return  _context.CourseComments.Where(c => c.CourseId == CourseId).Count();
        }

        public CourseCommentForAdminViweModel GetCommentUnRead(int PageId = 1, string trim="",bool checkajax=false)
        {
            IQueryable<CourseComment> result = _context.CourseComments.Include(c => c.User).Include(p => p.Course)
                .Where(p => p.IsAdminRead == false).AsNoTracking();
            if (!string.IsNullOrEmpty(trim))
            {
                result = result.Where(e => e.User.UserName.Contains(trim) || e.Comment.Contains(trim) || e.Course.CourseTitle.Contains(trim));
            }
            //Show Item In Page
            int take = 10;
            int skip = (PageId - 1) * take;
            CourseCommentForAdminViweModel vm = new CourseCommentForAdminViweModel
            {
                CurrentPage = PageId,
                checkajax=checkajax,
                trim=trim,
                PageCount = (int)Math.Ceiling(result.Count() / (double)take),
                courseComments = result.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList()
                
            };
            return vm;
                
        }

        public int UnreadComments()
        {
          return  _context.CourseComments.Where(p => p.IsAdminRead == false).Count();
        }

        public string UpdateComment(int CommentId, string Comment)
        {
            CourseComment comment = _context.CourseComments.Find(CommentId);
            comment.Comment = Comment;
            comment.IsAdminRead = true;
            _context.CourseComments.Update(comment);
            _context.SaveChanges();
            return Comment;
        }

        public bool ReadComment(List<int> CommentId)
        {
            foreach(var item in CommentId)
            {
                CourseComment comment = _context.CourseComments.Find(item);
                comment.IsAdminRead = true;
                _context.CourseComments.Update(comment);
            }
         
            _context.SaveChanges();
            return true;
        }

        public bool DeleteComment(int CommentId)
        {
            CourseComment comment = _context.CourseComments.Find(CommentId);
            _context.CourseComments.Remove(comment);
            _context.SaveChanges();
            return true;
        }

        public List<ShowCourseListItemViewModel> GetPopularCourse()
        {
            return _context.Courses.Include(c => c.OrderDetails)
               .Where(c => c.OrderDetails.Any())
               .OrderByDescending(d => d.OrderDetails.Count)
               .Take(8)
               .Select(c => new ShowCourseListItemViewModel()
               {
                   CourseId = c.CourseId,
                   ImageName = c.CourseImageName,
                   Price = c.CoursePrice,
                   Title = c.CourseTitle,
                   TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
               })
               .ToList();
        }

        public void AddGroup(CourseGroup group)
        {
            _context.CourseGroups.Add(group);
            _context.SaveChanges();
        }

        public void UpdateGroup(CourseGroup group)
        {
            _context.CourseGroups.Update(group);
            _context.SaveChanges();
        }

        public CourseGroup GetById(int groupId)
        {
            return _context.CourseGroups.Find(groupId);
        }

        public void DeleteGroup(CourseGroup group)
        {
            if (group.ParentId == null)
            {
                _context.CourseGroups.Remove(group);
                var list = _context.CourseGroups.Where(g => g.ParentId == group.GroupId);
                foreach (var item in list)
                {
                    _context.CourseGroups.Remove(item);
                    
                    
                }
                _context.SaveChanges();
            }
            else
            {
                _context.CourseGroups.Remove(group);
                _context.SaveChanges();
            }
                
            
        }

     
    }
}
