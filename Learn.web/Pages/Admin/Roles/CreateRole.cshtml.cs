using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Roles
{
    
    [PermissionChecker(7)]
    public class CreateRoleModel : PageModel
    {
        private IPermissionService _permissionService;
        public CreateRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        [BindProperty]
        public CreateRolesViewModel CreateRolesViewModel { get; set; }
        public void OnGet()
        {
            CreateRolesViewModel =_permissionService.CreateRole();
        }
        public IActionResult OnPost(List<int> SelectedPermission)
        {
            if (!ModelState.IsValid)
                return Page();


            CreateRolesViewModel.IsDelete = false;
            int roleId = _permissionService.AddRole(CreateRolesViewModel);

            _permissionService.AddPermissionsToRole(roleId, SelectedPermission);

            return Redirect("/Admin/Roles?Succes=CreateOk");
        }
    }
}