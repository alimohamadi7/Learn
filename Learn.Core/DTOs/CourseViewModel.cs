using Learn.Core.Extintions;
using Learn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.Core.DTOs
{
    public class CourseViewModel
    {
        [Key]
        public int CourseId { get; set; }
        [Display(Name = "نام گروه")]
        [Required(ErrorMessage = "لطفا {0} را مشخص کنید.")]
        public int GroupId { get; set; }

        public int? SubGroup { get; set; }
        [Display(Name = "نام استاد")]
        [Required(ErrorMessage = "لطفا {0} را مشخص کنید.")]
        public int TeacherId { get; set; }
        [Display(Name = " وضعیت دوره")]
        [Required(ErrorMessage = "لطفا {0} را مشخص کنید..")]
        public int StatusId { get; set; }
        [Display(Name = "سطح دوره")]
        [Required(ErrorMessage = "لطفا {0} را مشخص کنید.")]
        public int LevelId { get; set; }

        [Display(Name = "عنوان دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(450, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CourseTitle { get; set; }

        [Display(Name = "شرح دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string CourseDescription { get; set; }
        [Display(Name = "شرح مختصردوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CourseShortDescription { get; set; }
        [Display(Name = "قیمت دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int CoursePrice { get; set; }

        [MaxLength(600)]
        public string Tags { get; set; }

        [MaxLength(50)]
        public string CourseImageName { get; set; }

        [MaxLength(100)]
        public string DemoFileName { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        [Display(Name = "نظرات دوره")]
        public bool ShowComent { get; set; }
        public DateTime? UpdateDate { get; set; }
        [UploadFileExtensions(".png,.jpg,.jpeg,.gif", ErrorMessage = "لطفا یک عکس انتخاب نمایید")]
        [DataType(DataType.Upload)]
        public IFormFile imgCourseUp { get; set; }
        public IFormFile imgCourseUpEdit { get; set; }
        public IFormFile demoUp { get; set; }
        public SelectList Groups { get; set; }
        public SelectList SupGroups { get; set; }
        public SelectList Teachers { get; set; }
        public SelectList Levels { get; set; }
        public SelectList Stautus { get; set; }
    }
    public class CourseForIndexViweModel
    {
        public List<Course> Courses { get; set; }
        public int EpisodeCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string trim { get; set; }
        //for message in view
        public string Message { get; set; }
    }
    public class CourseEditViewModel
    {
        [Key]
        public int CourseId { get; set; }
        [Display(Name = "نام گروه")]
        [Required(ErrorMessage = "لطفا {0} را مشخص کنید.")]
        public int GroupId { get; set; }

        public int? SubGroup { get; set; }
        [Display(Name = "نام استاد")]
        [Required(ErrorMessage = "لطفا {0} را مشخص کنید.")]
        public int TeacherId { get; set; }
        [Display(Name = " وضعیت دوره")]
        [Required(ErrorMessage = "لطفا {0} را مشخص کنید..")]
        public int StatusId { get; set; }
        [Display(Name = "سطح دوره")]
        [Required(ErrorMessage = "لطفا {0} را مشخص کنید.")]
        public int LevelId { get; set; }

        [Display(Name = "عنوان دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(450, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CourseTitle { get; set; }

        [Display(Name = "شرح دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string CourseDescription { get; set; }
        [Display(Name = "شرح مختصردوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CourseShortDescription { get; set; }
        [Display(Name = "قیمت دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int CoursePrice { get; set; }
        [Display(Name = "نظرات دوره")]
        public bool ShowComent { get; set; }
        [MaxLength(600)]
        public string Tags { get; set; }

        [MaxLength(50)]
        public string CourseImageName { get; set; }

        [MaxLength(100)]
        public string DemoFileName { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public bool demoDelete { get; set; }
        public DateTime? UpdateDate { get; set; }
        public IFormFile imgCourseUpEdit { get; set; }
        public IFormFile demoUp { get; set; }
        public SelectList Groups { get; set; }
        public SelectList SupGroups { get; set; }
        public SelectList Teachers { get; set; }
        public SelectList Levels { get; set; }
        public SelectList Stautus { get; set; }
    }
    public class CourseCommentViewModel
    {
        public int  CommentId{ get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public string CreateDate { get; set; }
        public int? ParentId { get; set; }
        public string UserAvatar { get; set; }
        public bool Display { get; set; }
    }
    public class CourseCommentForAdminViweModel
    {
        public List<CourseComment> courseComments { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string trim { get; set; }
        public bool checkajax { get; set; }
    }
 
}
