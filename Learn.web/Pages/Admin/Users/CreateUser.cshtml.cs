using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.Convertors;
using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Users
{
    [PermissionChecker(3)]
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permissionService;
        public CreateUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }
        [BindProperty]
        public CreateUserViewModel CreateUserViewModel { get; set; }
        public void OnGet()
        {
            CreateUserViewModel = _userService.GetUserRolesForCeate();
            
        }
        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
            {
                CreateUserViewModel = _userService.GetUserRolesForCeate();
                
                return Page();
            }
                
            if (_userService.IsExistUserName(CreateUserViewModel.UserName))
            {
                CreateUserViewModel = _userService.GetUserRolesForCeate();
                ModelState.AddModelError("CreateUserViewModel.UserName", "نام کاربری قبلا انتخاب شده است ");
                return Page();
            }
            if (_userService.IsExistEmail(FixedText.FixEmail(CreateUserViewModel.Email)))
            {
                CreateUserViewModel = _userService.GetUserRolesForCeate();
                ModelState.AddModelError("CreateUserViewModel.Email", "ایمیل وارد شده قبلا ثبت نام شده است ");
                return Page();
            }
            if (!SelectedRoles.Any())
            {
                CreateUserViewModel = _userService.GetUserRolesForCeate();
                ViewData["ErrorSelectedRoles"]= true;
                return Page();
            }
            int userId = _userService.AddUserFromAdmin(CreateUserViewModel);
            //add roles
            _permissionService.AddRolesToUser(SelectedRoles, userId);
            return Redirect("/Admin/Users?Succes=CreateOk");
    }
    }
}