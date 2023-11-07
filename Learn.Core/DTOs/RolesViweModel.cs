using Learn.DataLayer.Entities.Permissions;
using Learn.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.Core.DTOs
{
    public class RolesViweModel
    {
        public List<Role> Roles { get; set; }
        public string Message { get; set; }
        

    }

    public class CreateRolesViewModel
    {
       
        public int RoleId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RoleTitle { get; set; }
        public bool IsDelete { get; set; }
        public List<Permission> permissions { get; set; }
        
    }
   public class EditRolesViewModel
    {

        public int RoleId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RoleTitle { get; set; }
        public bool IsDelete { get; set; }
        public List<Permission> permissions { get; set; }
        public List<int> SelectedPermissions { get; set; }
    }
}
