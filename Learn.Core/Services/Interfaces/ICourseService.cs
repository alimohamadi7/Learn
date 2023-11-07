using Learn.Core.DTOs;
using Learn.DataLayer.Entities.Course;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.Services.Interfaces
{
  public  interface ICourseService
    {
        #region Group

        List <CourseGroup>  GetAllGroup();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int? groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels();
        List<SelectListItem> GetStatues();
        CourseGroup GetById(int groupId);
        void AddGroup(CourseGroup group);
        void UpdateGroup(CourseGroup group); 
        void DeleteGroup(CourseGroup group);
        #endregion
        #region CrudCourseSrviceInformation
        CourseViewModel GetInformationCeraeteCourse(int? id);
        CourseEditViewModel GetInformationEditCourse(int id);
        CourseForIndexViweModel GetCoursesForAdmin(int PageId =1, string trim ="", string Succes ="");
        #endregion
        #region Course
        int AddCourse(CourseViewModel CourseViewModel);
        Course GetCourseById(int courseId);
        void UpdateCourse(CourseEditViewModel CourseViewModel);
        void DeleteCorse(Course course);
        Course GetCourseForDetails(int id);
        int CourseCount();
        List<ShowCourseListItemViewModel> GetLastcourses();
        Tuple<  List <ShowCourseListItemForArchiveViewModel>, int> GetCourse(int pageId = 1, string filter = "", string getType = "all",
        string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null, int take = 0);
        Course GetCourseForShow(int courseId);
        void CourseUpdateDate(int couseId);
        void UpdateCourseForUser(int CourseId, int userId);
        List<ShowCourseListItemViewModel> GetPopularCourse();
        #endregion
        #region Episode
        List<CourseEpisode> GetListEpisodeCorse(int courseId);
        bool CheckExistFile(string fileName);
        int AddEpisode(CourseEpisode episode, IFormFile episodeFile);
        CourseEpisode GetEpisodeById(int episodeId);
        void EditEpisode(CourseEpisode episode, IFormFile episodeFile);
        void DleteEpisode(CourseEpisode courseEpisode);
        #endregion
        #region Comments

        void AddComment(string comment,int courseId, int? parentId, int userId,int? OrderComment,int?commentId);
        int GetCommentCount(int CourseId);
       List<CourseCommentViewModel> GetCourseComment(int courseId, int pageindex , int pagesize,bool display);
        List<CourseCommentViewModel> GetCourseCommentForCreate(int CourseId);
        CourseCommentForAdminViweModel GetCommentUnRead(int PageId = 1,string trim="",bool checkajax=false);
        int UnreadComments();
        string UpdateComment(int CommentId, string Comment);
        bool ReadComment(List<int> CommentId);
        bool DeleteComment(int CommentId);
        #endregion

    }
}
