using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;

namespace informix.IDAL
{
    public interface IAuthModule
    {
        bool DeleteAuthModule(int moduleId);
        /// <summary>
        /// 添加应用模块
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool Add(Sys_Auth_Module authModule);
        /// <summary>
        /// 按模块编号、权限编号删除权限模块
        /// </summary>
        /// <param name="authorityId"></param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        bool DelAuthModuleByModuleId(int authorityId, int moduleId);
        /// <summary>
        /// 按权限编号删除权限模块
        /// </summary>
        /// <param name="authorityId"></param>
        /// <returns></returns>
        bool DelAuthModuleByAuthId(int authorityId);
    }
}
