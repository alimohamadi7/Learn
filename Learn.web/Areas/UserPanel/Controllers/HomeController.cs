using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Learn.Core.Convertors;
using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Learn.web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserService _userService;
   
        public HomeController(IUserService userService )
        {
            _userService = userService;
           
        }
        
        // [Route("UserPanel/{*username}")]
        public IActionResult Index(string EditProfile=null)
        {
            
            if (User.Identity.IsAuthenticated)
            {
               
                
                    string token = _userService.GettokenByUsername(User.Identity.Name);

                    if (token != User.FindFirst("Token").Value)
                    {
                        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        return Redirect("/Login?Message=OK");

                    }
                

            
            }

            //or another solution
            // return View(_userService.GetUserInformation(username));
            ViewData["EditProfile"] = EditProfile;
            return View(_userService.GetUserInformation(User.Identity.Name));

        }

        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            return View(_userService.GetDataForEditProfileUser(User.Identity.Name));
        }
        [Route("UserPanel/EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel profile)
        {
            if (!ModelState.IsValid)
                return View(profile);
                
                _userService.EditProfile(User.Identity.Name,profile);
                
               
            return Redirect("/UserPanel?EditProfile=Ok");

        }
        [Route("UserPanel/EditPhotoProfile")]
        public IActionResult EditPhotoProfile()
        {
            return View(_userService.GetDataForEditPhotoProfileUser(User.Identity.Name));
        }
        [HttpPost]
        [Route("UserPanel/EditPhotoProfile")]
        public IActionResult EditPhotoProfile(EditProfilePhotoViewModel profile)
        {
            if (!ModelState.IsValid)
                return View(profile);

            if (profile.UserAvatar.IsImage() == true)
            {
                if (profile.UserAvatar.ImageSize() == true)
                {

                    _userService.EditPhotoProfile(User.Identity.Name, profile);
                    
                    var user = User as ClaimsPrincipal;
                    var identity = user.Identity as ClaimsIdentity;

                    var claimNameList = identity.Claims.Select(x => x.Type).ToList();

                    foreach (var name in claimNameList)
                    {
                        if (name.Equals("ImageName") )
                        {
                            var claim = identity.Claims.FirstOrDefault(x => x.Type == name);
                            if (claim != null)
                                identity.RemoveClaim(claim);
                        }
                    }
                 
                    identity.AddClaim(new Claim("ImageName",profile.AvatarName));
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(principal);
                }
                else
                {
                    ModelState.AddModelError("UserAvatar", "سایز عکس بیشتر از مقدار قابل قبول است ");
                    return View(profile);
                }
            }
            else
            {
                ModelState.AddModelError("UserAvatar", "لطفا یک عکس انتخاب نمایید");

                return View(profile);
            }
            //Log Out User
            //HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/UserPanel?EditProfile=PhotoOk");

        }
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Route("UserPanel/ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            string currentUserName = User.Identity.Name;

            if (!ModelState.IsValid)
                return View(change);

            if (!_userService.CompareOldPassword(currentUserName, change.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمیباشد");
                return View(change);
            }

            _userService.ChangeUserPassword(currentUserName, change.Password);
            ViewBag.IsSuccess = true;

            return View();
        }
  
    }
}