using Learn.DataLayer.Entities.Course;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.DTOs
{
  public  class ShowCourseListItemViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Price { get; set; }
        public DateTime CreateDate { get; set; }
        public TimeSpan TotalTime { get; set; }
    }
    public class ShowCourseListItemForArchiveViewModel
    {
         public int CourseId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Price { get; set; }
        public DateTime CreateDate { get; set; }
        public TimeSpan TotalTime { get; set; }
        
    }
}
