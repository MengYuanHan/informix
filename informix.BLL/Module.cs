using informix.IDAL;
using informix.Model.Entities;
using System.Collections.Generic;
using System.Data;

namespace informix.BLL
{
    public class Module {
        private readonly IModule _dal = DALFactory.DataAccess.CreateModule();
        /// <summary>
        /// 得到模块列表
        /// </summary>
        /// <returns></returns>
        public string GetModuleList() => _dal.GetModuleList();
        /// <summary>
        /// 添加应用模块
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddAppModule(Sys_Module module) => _dal.AddAppModule(module);
        /// <summary>
        /// 获取模块代码
        /// </summary>
        /// <returns></returns>
        public string GetCode() => _dal.GetCode();
        /// <summary>
        /// 获取最大的模块id
        /// </summary>
        /// <returns></returns>
        public int GetMaxModuleId() => _dal.GetMaxModuleId();
        /// <summary>
        /// 根据应用id获取模块
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public int GetModuleIdByAppId(string applicationId) => _dal.GetModuleIdByAppId(applicationId);
        /// <summary>
        /// 通过应用Id删除模块
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public bool DelByAppId(string applicationId) => _dal.DelByAppId(applicationId);
        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Module> GetModule() => _dal.GetModule();
        /// <summary>
        /// 通过角色Id获取权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetAuthByRoleId(string roleId) => _dal.GetAuthByRoleId(roleId);
        /// <summary>
        /// 获取应用模块记录数
        /// </summary>
        /// <returns></returns>
        public int GetAppModuleCount() => _dal.GetAppModuleCount();
        //根据模块id更新状态
        public string UpdateAppModuleStatus(string moduleId, string close) => _dal.UpdateAppModuleStatus(moduleId, close);
        /// <summary>
        /// 根据应用Id查询信息
        /// </summary>
        /// <param name="appId">模块Id</param>
        /// <returns></returns>
        public IList<Sys_Module> GetAppModuleByAppId(string appId) => _dal.GetAppModuleByAppId(appId);
        /// <summary>
        /// 通过模块Id删除模块
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public bool DeleteAppModuleByModuleId(string moduleId) => _dal.DeleteAppModuleByModuleId(moduleId);
        /// <summary>
        /// 根据模块Id查询模块
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public DataTable GetAppModuleById(string moduleId) => _dal.GetAppModuleById(moduleId);
        /// <summary>
        /// 更新应用模块
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool UpdateAppModule(Sys_Module module) => _dal.UpdateAppModule(module);
    }
}
