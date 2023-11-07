using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Roles
{
    [PermissionChecker(8)]
    public class EditRoleModel : PageModel
    {
        private IPermissionService _permissionService;
        public EditRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        [BindProperty]
        public EditRolesViewModel EditRolesViewModel { get; set; }
        public void OnGet(int id)
        {
            EditRolesViewModel = _permissionService.EditRole(id);
        }
        public ActionResult OnPost(List<int> SelectedPermission)
        {
            if (!ModelState.IsValid)
                return Page();
            _permissionService.UpdateRole(EditRolesViewModel);
            _permissionService.UpdatePermissionsRole(EditRolesViewModel.RoleId, SelectedPermission);
            return Redirect("/Admin/Roles?Succes=EditOk");
        }
    }
}