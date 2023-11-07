using Learn.Core.DTOs;
using Learn.DataLayer.Entities.Permissions;
using Learn.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Learn.Core.Services.Interfaces
{
  public  interface IPermissionService
    {
        #region Roles
        List<Role> GetRoles();
        RolesViweModel RolesForView(string Succes = "");
        CreateRolesViewModel CreateRole();
        EditRolesViewModel EditRole(int id);
        int AddRole(CreateRolesViewModel CreateRolesViewModel);
        Role GetRoleById(int roleId);
        void DeleteRole(Role role);
        void UpdateRole(EditRolesViewModel role);
        void AddRolesToUser(List<int> roleIds, int userId);
        void EditRolesUser(int userId, List<int> rolesId);
        bool GetRoleByUsrname(int userId);
        bool GetRoleByUsrId(int userId);
        #endregion
        #region Permission

        List <Permission> GetAllPermission();
        void AddPermissionsToRole(int roleId, List<int> permission);
        List<int> PermissionsRole(int roleId);
        void UpdatePermissionsRole(int roleId, List<int> permissions);
        bool CheckPermission(int permissionId, int userId);
        
        #endregion
    }
}

