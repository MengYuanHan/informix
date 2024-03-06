using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;

namespace informix.IDAL
{
    public interface IUserRole
    {
        /// <summary>
        /// 添加用户角色关系
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool AddUserRole(Sys_User_Role userRole);
        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool DelUserRole(string roleId);
        /// <summary>
        /// 用户编号、角色编号删除用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool DelUserRoleByUR(string userId, string roleId);
        /// <summary>
        /// 判断用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int IsUserRole(string userId, string roleId);
    }
}
