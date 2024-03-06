using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;

namespace informix.IDAL
{
    public interface IAuthority
    {
        /// <summary>
        /// 根据用户编号、权限编号查询权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authorityId"></param>
        /// <returns></returns>
        string GetAuthByUserId(string userId, string authorityId);
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool AddAuthority(Sys_Authority authority);
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="authority"></param>
        /// <returns></returns>
        bool DeleteAuthority(string authority);
        /// <summary>
        /// 获取最大的权限编号
        /// </summary>
        /// <returns></returns>
        int GetMaxAuthorityId();
        /// <summary>
        /// 根据权限名获取权限编号
        /// </summary>
        /// <param name="authority"></param>
        /// <returns></returns>
        int GetAuthorityId(string authority);
    }
}
