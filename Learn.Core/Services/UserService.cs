
using Learn.Core.Convertors;
using Learn.Core.DTOs;
using Learn.Core.Generator;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Context;
using Learn.DataLayer.Entities.User;
using Learn.DataLayer.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

using Learn.DataLayer.Entities.Order;
using Learn.DataLayer.Entities.Course;
using Microsoft.EntityFrameworkCore.Internal;

namespace Learn.Core.Services
{
 
    public class UserService : IUserService
    {
        private LearnContext _context;
        private IPermissionService _permissionService;
       
        public UserService(LearnContext Context, IPermissionService permissionService)
        {
            _context = Context;
            _permissionService = permissionService;
           
        }

   
        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName.ToLower() == userName.ToLower());
        }
        public int AddUser(User user)
        {

            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User LoginUser(LoginViewModel login)
        {
            

            string HashPasword = PasswordHelper.EncodePasswordMd5(login.Password);
            string email = FixedText.FixEmail(login.Email);
            
            return (_context.Users.SingleOrDefault(u => u.Email == email && u.Password == HashPasword));
        }

        public bool ActiveAccount(string activeCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
            if (user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActiveCode = GenerateUniq.GenerateUniqCode();
            _context.SaveChanges();

            return true;
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);

        }

        public User GetUserByActiveCode(string activeCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activeCode);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public User GetUserByUserName(string username)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == username);
        }

        public InformationUserViewModel GetUserInformation(string username)
        {
            var user = GetUserByUserName(username);
            InformationUserViewModel information = new InformationUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                Wallet =BalanceUserWallet(username)
            };
            return information;
        }

        public SideBarUserPanelViewModel GetSideBarUserPanelData(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new SideBarUserPanelViewModel()
            {
                UserName = u.UserName,
                ImageName = u.UserAvatar,
                RegisterDate = u.RegisterDate
            }).Single();
        }

        public EditProfileViewModel GetDataForEditProfileUser(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new EditProfileViewModel()
            {
                
                BibiographyUser=u.Bibiography,
                LastNameUser=u.LastName,
                NameUser=u.Name,
                PhoneNumberUser=u.PhoneNumber,
                WebsiteUser=u.Website,
                
            }).Single();

        }
        public EditProfilePhotoViewModel GetDataForEditPhotoProfileUser(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new EditProfilePhotoViewModel()
            {
                AvatarName = u.UserAvatar,
        
            }).Single();
        }
        public void EditProfile(string username, EditProfileViewModel profile)
        {
            var sanitizer =HtmlSanitizer.SimpleHtml5DocumentSanitizer();
           
            var user = GetUserByUserName(username);
            if(!String.IsNullOrEmpty(profile.NameUser))
            user.Name =sanitizer.Sanitize(profile.NameUser);
            else user.Name = profile.NameUser;
            if (!String.IsNullOrEmpty(profile.LastNameUser))
                user.LastName = sanitizer.Sanitize(profile.LastNameUser);
            else user.LastName = profile.LastNameUser;
            user.PhoneNumber = profile.PhoneNumberUser;
            user.Website = profile.WebsiteUser;
            if (!String.IsNullOrEmpty(profile.BibiographyUser))
                user.Bibiography = sanitizer.Sanitize(profile.BibiographyUser);
            else user.Bibiography = profile.BibiographyUser;
                    UpdateUser(user);

        }
        public void EditPhotoProfile(string username, EditProfilePhotoViewModel profile)
        {
            if (profile.UserAvatar != null && profile.UserAvatar.IsImage())
            {
                string imagePath = "";
                if (profile.AvatarName != "user_avatar.png")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                profile.AvatarName = GenerateUniq.GenerateUniqCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }

            }
            var user = GetUserByUserName(username);
            user.UserAvatar = profile.AvatarName;
            UpdateUser(user);
        }
        public bool CompareOldPassword(string username, string oldPassword)
        {
            string hashpassword = PasswordHelper.EncodePasswordMd5(oldPassword);
            return _context.Users.Any(u => u.UserName == username  && u.Password == hashpassword);
        }

        public void ChangeUserPassword(string userName, string newPassword)
        {
            var user = GetUserByUserName(userName);
            user.Password = PasswordHelper.EncodePasswordMd5(newPassword);
            UpdateUser(user);
        }
        public int GetUserIdByUserName(string userName)
        {
            return _context.Users.Single(u => u.UserName == userName).UserId;
        }
        public int BalanceUserWallet(string userName)
        {
            var userId = GetUserIdByUserName(userName);
            var entercash = _context.Wallets
                .Where(w => w.UserId == userId && w.TypeId == 1 && w.IsPay)
                .Select(w =>w.Amount).ToList();
            var exitecash = _context.Wallets
                .Where(w => w.UserId == userId && w.TypeId == 2)
                .Select(w=>w.Amount).ToList();
            return (entercash.Sum() - exitecash.Sum());
        }

        public List<WalletViewModel> GetWalletUser(string userName)
        {
            var userId = GetUserIdByUserName(userName);
          return   _context.Wallets.Where(w => w.IsPay && w.UserId == userId).Take(10)
                .Select(w=>new WalletViewModel
                {
                    Amount=w.Amount,
                    DateTime=w.CreateDate,
                    Description=w.Description,
                    Type=w.TypeId
                }
                ).ToList();
 
        }

        public int ChargeWallet(string userName, int amount, string description, bool isPay = false)
        {
            Wallet wallet = new Wallet()
            {
                Amount=amount,
                IsPay=isPay,
                Description=description,
                TypeId = 1,
                CreateDate=DateTime.Now,
                UserId =GetUserIdByUserName(userName)
            };
         return AddWallet(wallet);
            
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.Find(walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }

        public UserForAdminViewModel GetUser(int PageId = 1, string trim = "",string Succes="")
        {
            IQueryable<User> result = _context.Users.Include(u=>u.UserRoles).ThenInclude(u=>u.Role).Where(u=>u.IsDelete==false);

            if (!string.IsNullOrEmpty(trim))
            {
                result = result.Where(u => u.UserName.Contains(trim) || u.Email.Contains(trim));
            }
            //Show Item In Page
            int take = 10;
            int skip = (PageId - 1) * take;
            UserForAdminViewModel user = new UserForAdminViewModel()
            {
                CurrentPage = PageId,
                PageCount = (int)Math.Ceiling(result.Count() /(double) take),
                trim=trim,
                 Message=Succes,
                Users = result.OrderByDescending(c => c.RegisterDate).Skip(skip).Take(take).ToList()
                
            };
            return user;
        }

        public int AddUserFromAdmin(CreateUserViewModel user)
        {
            User addUser = new User();
            addUser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            addUser.ActiveCode = GenerateUniq.GenerateUniqCode();
            addUser.Email = user.Email;
            addUser.IsActive = true;
            addUser.RegisterDate = DateTime.Now;
            addUser.UserName = user.UserName;
            addUser.Name = user.NameUser;
            addUser.LastName = user.LastNameUser;
            addUser.PhoneNumber = user.PhoneNumberUser;
            addUser.Website = user.WebsiteUser;
            addUser.Bibiography = user.BibiographyUser;
            addUser.Token = Guid.NewGuid().ToString("N");
            #region Save Avatar

            if (user.UserAvatar != null)
            {
                string imagePath = "";
                addUser.UserAvatar = GenerateUniq.GenerateUniqCode() + Path.GetExtension(user.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", addUser.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    user.UserAvatar.CopyTo(stream);
                }
            }

            #endregion  
            return AddUser(addUser);
        }

        public UserforDetailsViewModel GetUserByIdForDetails(int Id)
        {
           
            UserforDetailsViewModel user =new UserforDetailsViewModel();
           user.users= _context.Users.Where(u => u.UserId == Id).Include(u=>u.UserCourses).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).ToList();
            user.walletUser = GetWalletUser(GetUserById(Id).UserName);
            user.WalletMoney = BalanceUserWallet(GetUserById(Id).UserName);
            user.userorder = GetUserOrdersWhitDetails(Id);
            return user;
        }

        public EditUserViewModel GetUserForShowInEditMode(int Id)
        {

            return _context.Users.Where(u => u.UserId == Id).Select(u => new EditUserViewModel()
            {
                BibiographyUser = u.Bibiography,
                Email = u.Email,
                LastNameUser = u.LastName,
                NameUser = u.Name,
                PhoneNumberUser = u.PhoneNumber,
                WebsiteUser = u.Website,
                AvatarName = u.UserAvatar,
                UserId = u.UserId,
                UserName = u.UserName,
                IsActive = u.IsActive,
                CurrentEmail=u.Email,
                CurrentUserName=u.UserName,
                UserRoles = u.UserRoles.Select(r => r.RoleId).ToList(),
                Roles = _permissionService.GetRoles()
            }).Single();
          
        }

        public CreateUserViewModel GetUserRolesForCeate()
        {
            List<Role> Roles = _permissionService.GetRoles();
            CreateUserViewModel createUserViewModel = new CreateUserViewModel();
            createUserViewModel.Roles = Roles;
            return createUserViewModel;
        }

        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user =GetUserById(editUser.UserId);
            user.Email = editUser.Email;
            user.IsActive = editUser.IsActive;
            user.UserName = editUser.UserName;
            user.Website = editUser.WebsiteUser;
            user.Bibiography = editUser.BibiographyUser;
            user.PhoneNumber = editUser.PhoneNumberUser;
            user.Name = editUser.NameUser;
            user.LastName = editUser.LastNameUser;
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUser.Password);
            }

            if (editUser.UserAvatar != null)
            {
                //Delete old Image
                if (editUser.AvatarName !="user_avatar.png")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                }

                //Save New Image
                user.UserAvatar = GenerateUniq.GenerateUniqCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editUser.UserAvatar.CopyTo(stream);
                }
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User GetUserById(int Id)
        {
            return _context.Users.Find(Id);
        }

        public InformationUserViewModel GetUserInformation(int userId)
        {
            var user = GetUserById(userId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.RegisterDate = user.RegisterDate;
            information.Wallet = BalanceUserWallet(user.UserName);

            return information;
        }

        public void DeleteUser(int userId)
        {
            User user = GetUserById(userId);
            user.IsDelete = true;
            UpdateUser(user);
        }
        public void Recoveryuser(int userId)
        {
            User user = GetUserById(userId);
            user.IsDelete = false;
            UpdateUser(user);
        }
        public UserForAdminViewModel GetDeletetUser(int PageId = 1, string trim = "", string Succes = "")
        {
            IQueryable<User> result = _context.Users.Where(u =>u.IsDelete==true).Include(u => u.UserRoles).ThenInclude(u => u.Role);

            if (!string.IsNullOrEmpty(trim))
            {
                result = result.Where(u => u.UserName.Contains(trim) || u.Email.Contains(trim));
            }
            //Show Item In Page
            int take = 10;
            int skip = (PageId - 1) * take;
            UserForAdminViewModel user = new UserForAdminViewModel()
            {
                CurrentPage = PageId,
                PageCount = (int)Math.Ceiling(result.Count() / (double)take),
                trim = trim,
                Message = Succes,
                Users = result.OrderByDescending(c => c.RegisterDate).Skip(skip).Take(take).ToList()

            };
            return user;
        }
        public List<userOrderViewmodel> GetUserOrdersWhitDetails(int id)
        {
            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course).Where(u => u.UserId == id).Select(u => new userOrderViewmodel()
            {

                orderId = u.OrderId,
                CreateDate=u.CreateDate,
                IsFinaly=u.IsFinaly,
                OrderSum=u.OrderSum,
                
            }).ToList();
          
            
        }

        public List<string> userOrderCourse(int orderId)
        {
            return _context.OrderDetails.Include(o => o.Course).Where(c => c.OrderId == orderId)
                .Select(c => c.Course.CourseTitle).ToList();
        }

        public List<string> UserCourse(int userId)
        {
            return _context.UserCourses.Include(c => c.Course)
                 .Where(c => c.UserId == userId).Select(c => c.Course.CourseTitle).ToList();
        }

        public string GettokenByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == username).Token;
        }

        public void ChangeToken(int userId)
        {
           User user= _context.Users.Find(userId);
            user.Token = Guid.NewGuid().ToString("N");
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
