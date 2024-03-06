using informix.IDAL;
using informix.Model.Entities;
using System.Collections.Generic;

namespace informix.BLL
{
    public class Role{
        private readonly IRole _dal = DALFactory.DataAccess.CreateRole();
        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <returns></returns>
        public string GetRoleAuth() => _dal.GetRoleAuth();
        /// <summary>
        /// 获取角色并分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IList<Sys_Role> GetRole(int currentPage, int pageCount) => _dal.GetRole(currentPage, pageCount);
        /// <summary>
        /// 获取用户数
        /// </summary>
        /// <returns></returns>
        public int GetUserCount() => _dal.GetRoleCount();
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddRole(Sys_Role role) => _dal.AddRole(role);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool DelRole(string roleId) => _dal.DelRole(roleId);
        /// <summary>
        /// 获取角色编号最大值
        /// </summary>
        /// <returns></returns>
        public int GetMaxRoleId() => _dal.GetMaxRoleId();
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Role> GetRole() => _dal.GetRole();
        /// <summary>
        /// 获取剩余角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public string GetSurplusRole(string userId) => _dal.GetSurplusRole(userId);
        /// <summary>
        /// 获取当前用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetCurrentUserRole(string userId) => _dal.GetCurrentUserRole(userId);
        /// <summary>
        /// 根据角色编号获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetRoleByRoleId(int roleId) => _dal.GetRoleByRoleId(roleId);
    }
}
