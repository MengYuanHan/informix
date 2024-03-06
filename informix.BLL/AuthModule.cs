using informix.IDAL;
using informix.Model.Entities;

namespace informix.BLL
{
    public class AuthModule{
        private readonly IAuthModule _dal = DALFactory.DataAccess.CreateAuthModule();
        /// <summary>
        /// 添加权限模块
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Add(Sys_Auth_Module authModule) => _dal.Add(authModule);
        /// <summary>
        /// 删除权限模块
        /// </summary>
        /// <param name="moduleId">模块编号</param>
        /// <returns></returns>
        public bool DeleteAuthModule(int moduleId) => _dal.DeleteAuthModule(moduleId);
        /// <summary>
        /// 删除权限模块
        /// </summary>
        /// <param name="authorityId">权限编号</param>
        /// <returns></returns>
        public bool DelAuthModuleByAuthId(int authorityId) => _dal.DelAuthModuleByAuthId(authorityId);
        /// <summary>
        /// 删除权限模块
        /// </summary>
        /// <param name="authorityId">权限编号</param>
        /// <param name="moduleId">模块编号</param>
        /// <returns></returns>
        public bool DelAuthModuleByModuleId(int authorityId, int moduleId) => _dal.DelAuthModuleByModuleId(authorityId, moduleId);
    }
}
