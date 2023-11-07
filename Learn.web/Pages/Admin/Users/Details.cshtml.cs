using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.DTOs;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Users
{
    public class DetailsModel : PageModel
    {
        private IUserService _userservice;
        public DetailsModel(IUserService userService)
        {
            _userservice = userService;
        }
        
        public UserforDetailsViewModel userforDetails { get; set; }
        
        public void OnGet(int id)
        {   
            userforDetails= _userservice.GetUserByIdForDetails(id);
        }
    
    }
}