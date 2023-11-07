
using Learn.Core.Extintions;
using Learn.DataLayer.Entities.Order;
using Learn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Learn.Core.DTOs
{
    public class UserForAdminViewModel
    {
        public List<User> Users { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string trim { get; set; }
        //for message in view
        public string Message{ get; set; }
    }
    public class CreateUserViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression(@"^[a-zA-Z0-9-']*$", ErrorMessage = "نام کاربری معتبر نمیباشد")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = " کلمه عبور نامعتبر ")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Password { get; set; }
        [Display(Name = "نام")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string NameUser { get; set; }
        [Display(Name = "نام خانوادگی")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string LastNameUser { get; set; }
        [Display(Name = "وب سایت")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [RegularExpression(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$", ErrorMessage = "وب سایت معتبر نیست")]
        public string WebsiteUser { get; set; }
        [Display(Name = "درباره من")]
        [MaxLength(700, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BibiographyUser { get; set; }
        [Display(Name = "شماره تماس")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [RegularExpression(@"(\+98|0)?9\d{9}$", ErrorMessage = "شماره تلفن معتبر نمیباشد")]
        public string PhoneNumberUser { get; set; }
        [UploadFileExtensions(".png,.jpg,.jpeg,.gif", ErrorMessage = "لطفا یک عکس انتخاب نمایید")]
        [DataType(DataType.Upload)]

    
        public IFormFile UserAvatar { get; set; }
        public List<Role> Roles { get; set; }
       
    }
    public class userOrderViewmodel
    {
        public int orderId { get; set; }
        public bool IsFinaly { get; set; }
        public DateTime CreateDate { get; set; }
        public int OrderSum { get; set; }
        //public List<OrderDetail> Courseuser { get; set; }
    }
    public class UserforDetailsViewModel
    {
        public List<User> users;
        public List<WalletViewModel> walletUser;
        public List<userOrderViewmodel> userorder;
        public int WalletMoney { get; set; }
    }
    public class EditUserViewModel
    {
        public int UserId { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression(@"^[a-zA-Z0-9-']*$", ErrorMessage = "نام کاربری معتبر نمیباشد")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string UserName { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = " کلمه عبور نامعتبر ")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Password { get; set; }
        [Display(Name = "نام")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string NameUser { get; set; }
        [Display(Name = "نام خانوادگی")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string LastNameUser { get; set; }
        [Display(Name = "وب سایت")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [RegularExpression(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$", ErrorMessage = "وب سایت معتبر نیست")]
        public string WebsiteUser { get; set; }
        [Display(Name = "درباره من")]
        [MaxLength(700, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string BibiographyUser { get; set; }
        [Display(Name = "شماره تماس")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [RegularExpression(@"(\+98|0)?9\d{9}$", ErrorMessage = "شماره تلفن معتبر نمیباشد")]
        public string PhoneNumberUser { get; set; }
        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }
        public string CurrentUserName { get; set; }
        public string CurrentEmail{ get; set; }
        public IFormFile UserAvatar { get; set; }
        public string AvatarName { get; set; }
        public List<int> UserRoles { get; set; }
        public List<Role> Roles { get; set; }
    }
}

