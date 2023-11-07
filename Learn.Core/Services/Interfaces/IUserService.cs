using Learn.Core.DTOs;
using Learn.DataLayer.Entities.Course;
using Learn.DataLayer.Entities.Order;
using Learn.DataLayer.Entities.User;
using Learn.DataLayer.Entities.Wallet;

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;


namespace Learn.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserByUserName(string username);
        User GetUserByActiveCode(string activeCode);
        void UpdateUser(User user);
        bool ActiveAccount(string activeCode);
        int GetUserIdByUserName(string userName);
        void DeleteUser(int userId);
        void Recoveryuser(int userId);
        string GettokenByUsername(string username);
        
        #region User Panel

        InformationUserViewModel GetUserInformation(string username);
        InformationUserViewModel GetUserInformation(int userId);
        SideBarUserPanelViewModel GetSideBarUserPanelData(string username);
        EditProfileViewModel GetDataForEditProfileUser(string username);
        EditProfilePhotoViewModel GetDataForEditPhotoProfileUser(string username);
        void EditProfile(string username, EditProfileViewModel profile);
        void EditPhotoProfile(string username, EditProfilePhotoViewModel profile);

        bool CompareOldPassword(string username, string oldPassword);

        void ChangeUserPassword(string userName, string newPassword);

        #endregion
        #region Wallet
        int BalanceUserWallet(string userName);
        List<WalletViewModel> GetWalletUser(string userName);
        int ChargeWallet(string userName, int amount, string description, bool isPay = false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);
        #endregion
        #region AdminUser
        UserForAdminViewModel GetUser(int PageId = 1, string trim = "", string Succes = "");
        UserForAdminViewModel GetDeletetUser(int PageId = 1, string trim = "", string Succes = "");
        int AddUserFromAdmin(CreateUserViewModel user);
        UserforDetailsViewModel GetUserByIdForDetails(int Id);
        CreateUserViewModel GetUserRolesForCeate();
        EditUserViewModel GetUserForShowInEditMode(int Id);
        User GetUserById(int Id);
        void EditUserFromAdmin(EditUserViewModel editUser);
        List<userOrderViewmodel> GetUserOrdersWhitDetails(int id);
        List<string> userOrderCourse(int orderId);
        List<string> UserCourse(int userId);
        void ChangeToken(int userId);
        #endregion

    }
}
