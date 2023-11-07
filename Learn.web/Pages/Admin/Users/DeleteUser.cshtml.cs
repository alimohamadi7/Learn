using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Users
{
    [PermissionChecker(5)]
    public class DeleteUserModel : PageModel
    {
        private IUserService _userServise;
        public DeleteUserModel(IUserService userServise)
        {
            _userServise = userServise;
        }
        public InformationUserViewModel InformationUserViewModel { get; set; }

        public void OnGet(int id)
        {
            ViewData["UserId"] = id;
            InformationUserViewModel = _userServise.GetUserInformation(id);
        }
        public IActionResult OnPost(int UserId)
        {
            _userServise.DeleteUser(UserId);
            return Redirect("/Admin/Users?Succes=DeleteOk");
        }
    }
}