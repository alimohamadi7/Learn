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
    public class ListDeleteUserModel : PageModel
    {
        private IUserService _userService;
        public ListDeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }
        public UserForAdminViewModel UserForAdminViewModel { get; set; }
        public void OnGet(int PageId = 1, string trim = "", string Succes = "")
        {

            UserForAdminViewModel = _userService.GetDeletetUser(PageId, trim, Succes);

        }
    }
}