using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.DTOs;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Learn.Core.Convertors;
using Learn.Core.Security;

namespace Learn.web.Pages.Admin.Users
{
    [PermissionChecker(4)]
    public class EditModel : PageModel
    {
        private IPermissionService _permissionService;
        private IUserService _userService;
            public EditModel(IPermissionService permissionService, IUserService userService)
        {
            _permissionService = permissionService;
            _userService = userService;
        }
        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }
        public void OnGet(int id)
        {
            EditUserViewModel = _userService.GetUserForShowInEditMode(id);
        }
        public ActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
            {
                EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);

                return Page();
            }
            if (EditUserViewModel.IsActive == false)
            {
                _userService.ChangeToken(EditUserViewModel.UserId);
            }
            if (EditUserViewModel.UserName== EditUserViewModel.CurrentUserName)
            {
                if (EditUserViewModel.Email == EditUserViewModel.CurrentEmail)
                {
                    _userService.EditUserFromAdmin(EditUserViewModel);
                    _permissionService.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);
                    return Redirect("/Admin/Users?Succes=EditOk");
                }
                else if(_userService.IsExistEmail(EditUserViewModel.Email))
                {
                    EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);
                    ModelState.AddModelError("EditUserViewModel.Email", "نام کاربری قبلا انتخاب شده است ");
                    return Page();
                }
                else
                {
                    _userService.EditUserFromAdmin(EditUserViewModel);
                    _permissionService.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);
                    return Redirect("/Admin/Users?Succes=EditOk");
                }
            }
            if(EditUserViewModel.Email == EditUserViewModel.CurrentEmail)
            {
                if (EditUserViewModel.UserName == EditUserViewModel.CurrentUserName)
                {
                    _userService.EditUserFromAdmin(EditUserViewModel);
                    _permissionService.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);
                    return Redirect("/Admin/Users?Succes=EditOk");
                }
                else if (_userService.IsExistUserName(EditUserViewModel.UserName))
                {
                    EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);
                    ModelState.AddModelError("EditUserViewModel.UserName", "نام کاربری قبلا انتخاب شده است ");
                    return Page();
                }
                else
                {
                    _userService.EditUserFromAdmin(EditUserViewModel);
                    _permissionService.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);
                    return Redirect("/Admin/Users?Succes=EditOk");
                }
            }
            if (_userService.IsExistUserName(EditUserViewModel.UserName))
            {
                EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);
                ModelState.AddModelError("EditUserViewModel.UserName", "نام کاربری قبلا انتخاب شده است ");
                return Page();
            }
            if (_userService.IsExistEmail(FixedText.FixEmail(EditUserViewModel.Email)))
            {
                EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);
                ModelState.AddModelError("EditUserViewModel.Email", "ایمیل وارد شده قبلا ثبت نام شده است ");
                return Page();
            }
            if (!SelectedRoles.Any())
            {
                EditUserViewModel = _userService.GetUserForShowInEditMode(EditUserViewModel.UserId);
                ViewData["ErrorSelectedRoles"]=true;
                return Page();
            }
            _userService.EditUserFromAdmin(EditUserViewModel);

            //Edit Roles
            _permissionService.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);
            return Redirect("/Admin/Users?Succes=EditOk");
        }
    }
}