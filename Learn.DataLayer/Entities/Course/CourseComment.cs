using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Learn.DataLayer.Entities.Course
{
    public class CourseComment
    {
        [Key]
        public int CommentId { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }

        
        public int? OrderComment { get; set; }
        [MaxLength(700)]
        [Required(ErrorMessage ="دیدگاه خود را درج نمایید.")]
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
       
        public bool IsAdminRead { get; set; }
        public  int? ParentId { get; set; }
        public Course Course { get; set; }
        public User.User User { get; set; }
        [ForeignKey("ParentId")]
        public virtual ICollection<CourseComment> CourseComments { get; set; }
    }
}
