using Learn.Core.DTOs;
using Learn.DataLayer.Context;
using Learn.DataLayer.Entities.Permissions;
using Learn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Learn.Core.Services.Interfaces
{
    public class PermissionService : IPermissionService
    {
        private LearnContext _Context;
     
        
        public PermissionService(LearnContext context )
        {
            _Context = context;

           
        }

        public List<Role> GetRoles()
        {

            return _Context.Roles.Include(p => p.RolePermissions).ThenInclude(p => p.Permission).ToList();
        }
        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (int roleId in roleIds)
            {
                _Context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });

            }
            _Context.SaveChanges();
        }
        public void EditRolesUser(int userId, List<int> rolesId)
        {
            //Delete All Roles User
            _Context.UserRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _Context.UserRoles.Remove(r));

            //Add New Roles
            AddRolesToUser(rolesId, userId);
        }

        public RolesViweModel RolesForView(string Succes = "")
        {
            RolesViweModel rolesViweModel = new RolesViweModel();
            List<Role> roles = GetRoles().Where(p => p.IsDelete == false).ToList();
            rolesViweModel.Roles = roles;
            rolesViweModel.Message = Succes;
            return rolesViweModel;
        }

        public int AddRole(CreateRolesViewModel CreateRolesViewModel)
        {
            Role role = new Role
            {
                IsDelete = false,
                RoleTitle = CreateRolesViewModel.RoleTitle

            };
            _Context.Roles.Add(role);
            _Context.SaveChanges();
            return role.RoleId;
        }

        public void UpdateRole(EditRolesViewModel Editrole)
        {
            Role role = GetRoleById(Editrole.RoleId);

            role.IsDelete = false;
            role.RoleTitle = Editrole.RoleTitle;

            _Context.Roles.Update(role);
            _Context.SaveChanges();
        }

        public Role GetRoleById(int roleId)
        {
            return _Context.Roles.Find(roleId);
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            _Context.Update(role);
            _Context.SaveChanges();
        }

        public List<Permission> GetAllPermission()
        {
            return _Context.Permission.ToList();
        }

        public void AddPermissionsToRole(int roleId, List<int> permission)
        {
            foreach (var p in permission)
            {
                _Context.RolePermission.Add(new RolePermission()
                {
                    PermissionId = p,
                    RoleId = roleId
                });
            }
            _Context.SaveChanges();
        }

        public List<int> PermissionsRole(int roleId)
        {
            return _Context.RolePermission.Where(r => r.RoleId == roleId)
                  .Select(r => r.PermissionId).ToList();
        }

        public void UpdatePermissionsRole(int roleId, List<int> permissions)
        {
            _Context.RolePermission.Where(p => p.RoleId == roleId)
               .ToList().ForEach(p => _Context.RolePermission.Remove(p));

            AddPermissionsToRole(roleId, permissions);
        }

        public CreateRolesViewModel CreateRole()
        {
            CreateRolesViewModel createRolesViewModel = new CreateRolesViewModel
            {
                permissions = GetAllPermission()
            };
            return createRolesViewModel;
        }

        public EditRolesViewModel EditRole(int id)
        {
            Role Rol = GetRoleById(id);
            EditRolesViewModel EditRolesViewModel = new EditRolesViewModel
            {
                permissions = GetAllPermission(),
                IsDelete = Rol.IsDelete,
                RoleId = Rol.RoleId,
                RoleTitle = Rol.RoleTitle,
                SelectedPermissions = PermissionsRole(id)
            };
            return EditRolesViewModel;
        }

        public bool CheckPermission(int permissionId, int userId)
        {
        

            List<int> UserRoles = _Context.UserRoles
                .Where(r => r.UserId == userId).Select(r => r.RoleId).ToList();

            if (!UserRoles.Any())
                return false;

            List<int> RolesPermission = _Context.RolePermission
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId).ToList();

            return RolesPermission.Any(p => UserRoles.Contains(p));
        }

        public bool GetRoleByUsrname(int userId)
        {
            bool returnRole = false;  
            if (_Context.UserRoles.Any(r => r.UserId == userId && r.RoleId != 3))
                returnRole = true;
            return returnRole;

        }

        public bool GetRoleByUsrId(int userId)
        {
            bool returnRole = false;
            if (_Context.UserRoles.Any(r => r.UserId == userId && (r.RoleId == 1||r.RoleId==2)))
                returnRole = true;
            return returnRole;
        }
    }
}