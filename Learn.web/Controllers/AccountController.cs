using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Learn.Core.Convertors;
using Learn.Core.DTOs;
using Learn.Core.Generator;
using Learn.Core.Security;
using Learn.Core.Senders;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Learn.web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userservice;
        private IViewRenderService _viewRender;
      
        public AccountController(IUserService userService, IViewRenderService viewRender)
        {
            _userservice = userService;
            _viewRender = viewRender;
        }
        #region Register

        
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            if (_userservice.IsExistUserName(register.UserName.Trim()))
            {
                ModelState.AddModelError("UserName", "نام کاربری قبلا انتخاب شده است ");
                return View(register);
            }
            if (_userservice.IsExistEmail(FixedText.FixEmail(register.Email))) 
            {
                ModelState.AddModelError("Email", "ایمیل وارد شده قبلا ثبت نام شده است ");
                return View(register);
            }
            User user = new User() {
                UserName = register.UserName,
                Email = FixedText.FixEmail(register.Email),
                ActiveCode = GenerateUniq.GenerateUniqCode(),
                IsActive = false,
                Password = PasswordHelper.EncodePasswordMd5( register.Password),
                UserAvatar = "user_avatar.png",
                RegisterDate = DateTime.Now,
                Token=Guid.NewGuid().ToString("N")
            };
            _userservice.AddUser(user);
            #region Send Activation Email

            string body = _viewRender.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email, "فعالسازی", body);

            #endregion

            return View("SuccessRegister",user);
        }
        #endregion
        #region Login
        [Route("Login")]
        public IActionResult Login(string ReturnUrl, string Message)
        {
            
            ViewBag.returnurl = ReturnUrl;
            
            ViewBag.Message = Message;
            return View();
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginViewModel login,string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                
                return View(login);
            }
            var user = _userservice.LoginUser(login );
            if (user != null)
            {
                if (!user.IsDelete)
                {
                    if (user.IsActive)
                    {
                        var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Email,user.Email),
                        new Claim("ImageName",user.UserAvatar),
                        new Claim("Token",user.Token),
                  
                    };
                       
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe
                        };
                        HttpContext.SignInAsync(principal, properties);
                        
                        ViewBag.IsSuccess = true;
                        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "کاربری با ایمیل وارد شده یافت نگردید");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "کاربری باایمیل واردشده یافت نگردید، رمزعبور یاایمیل را بررسی نمایید");
            }
            return View(login);
        }
        #endregion
        #region Active Account

        public IActionResult ActiveAccount(string id)
        {
            ViewBag.IsActive = _userservice.ActiveAccount(id);
            return View();
        }

        #endregion
        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion
        #region Forgot Password
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
                return View(forgot);

            string fixedEmail = FixedText.FixEmail(forgot.Email);
            DataLayer.Entities.User.User user = _userservice.GetUserByEmail(fixedEmail);

            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری یافت نشد");
                return View(forgot);
            }

            string bodyEmail = _viewRender.RenderToStringAsync("_ForgotPassword", user);
            SendEmail.Send(user.Email, "بازیابی حساب کاربری", bodyEmail);
            ViewBag.IsSuccess = true;

            return View();
        }
        #endregion

        #region Reset Password

        public ActionResult ResetPassword(string id)
        {
            
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
            });
        }


        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
                return View(reset);

            DataLayer.Entities.User.User user = _userservice.GetUserByActiveCode(reset.ActiveCode);

            if (user == null)
                return NotFound();

            string hashNewPassword = PasswordHelper.EncodePasswordMd5(reset.Password);
            user.Password = hashNewPassword;
            _userservice.UpdateUser(user);
            ViewBag.IsSuccess = true;
            return View();
           

        }
        #endregion

    }
}