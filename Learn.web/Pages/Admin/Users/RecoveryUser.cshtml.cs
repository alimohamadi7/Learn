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
    [PermissionChecker(2)]
    public class RecoveryUserModel : PageModel
    {
        private IUserService _userServise;
        public RecoveryUserModel(IUserService userServise)
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
            _userServise.Recoveryuser(UserId);
            return RedirectToPage("ListDeleteUser");
        }
    }
}