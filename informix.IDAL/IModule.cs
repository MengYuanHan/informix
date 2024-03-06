using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;


namespace informix.IDAL
{
    public interface IModule
    {
        /// <summary>
        /// 查询所有模块信息
        /// </summary>
        /// <returns></returns>
        string GetModuleList();
        /// <summary>
        /// 添加应用模块
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool AddAppModule(Sys_Module module);
        /// <summary>
        /// 获取模块代码
        /// </summary>
        /// <returns></returns>
        string GetCode();
        /// <summary>
        /// 获取模块Id最大值
        /// </summary>
        /// <returns></returns>
        int GetMaxModuleId();
        /// <summary>
        /// 按应用Id查询模块Id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        int GetModuleIdByAppId(string applicationId);
        /// <summary>
        /// 通过应用删除
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        bool DelByAppId(string applicationId);
        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <returns></returns>
        IList<Sys_Module> GetModule();
        /// <summary>
        /// 获取权限通过角色Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        string GetAuthByRoleId(string roleId);
        /// <summary>
        /// 通过应用编号获得应用模块
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        IList<Sys_Module> GetAppModuleByAppId(string appId);
        /// <summary>
        /// 获取应用模块数
        /// </summary>
        /// <returns></returns>
        int GetAppModuleCount();
        /// <summary>
        /// 更新模块状态
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="close"></param>
        /// <returns></returns>
        string UpdateAppModuleStatus(string moduleId,string close);
        /// <summary>
        /// 根据模块编号删除应用模块
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        bool DeleteAppModuleByModuleId(string moduleId);
        /// <summary>
        /// 根据模块编号查询模块
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        DataTable GetAppModuleById(string moduleId);
        /// <summary>
        /// 更新应用模块
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        bool UpdateAppModule(Sys_Module obj);
    }
}
