using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;

namespace informix.IDAL
{
    public interface IAuthRole
    {
        /// <summary>
        /// 添加权限角色关系
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool AddAuthRole(Sys_Auth_Role authRole);
        /// <summary>
        /// 删除权限角色关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool DelAuthRole(string roleId);
        /// <summary>
        /// 根据角色编号、模块编号判断权限角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        int IsAuthRole(string roleId,string moduleId);
        /// <summary>
        /// 根据角色编号查出权限编号
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int GetAuthorityIdByRoleId(int roleId);
    }
}
