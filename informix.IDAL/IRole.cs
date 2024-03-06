using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;

namespace informix.IDAL
{
    public interface IRole
    {
        /// <summary>
        /// 获取角色权限关系
        /// </summary>
        /// <returns></returns>
        string GetRoleAuth();
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        IList<Sys_Role> GetRole(int currentPage, int pageCount);
        /// <summary>
        /// 获取角色数
        /// </summary>
        /// <returns></returns>
        int GetRoleCount();
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool AddRole(Sys_Role role);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool DelRole(string roleId);
        /// <summary>
        /// 获取角色编号最大值
        /// </summary>
        /// <returns></returns>
        int GetMaxRoleId();
        /// <summary>
        /// 获得所有角色
        /// </summary>
        /// <returns></returns>
        IList<Sys_Role> GetRole();
        /// <summary>
        /// 根绝角色编号获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        string GetRoleByRoleId(int roleId);
        /// <summary>
        /// 查询剩余角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetSurplusRole(string userId);
        /// <summary>
        /// 查询当前用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetCurrentUserRole(string userId);
    }
}
