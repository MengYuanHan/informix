using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;
using System.Collections;

namespace informix.IDAL
{
    public interface IApplication
    {
        /// <summary>
        /// 添加应用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool AddApp(Sys_Application application);
        /// <summary>
        /// 通过编号删除应用
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        bool DelAppById(string applicationId);
        /// <summary>
        /// 获取所有应用[添加模块时使用]
        /// </summary>
        /// <returns></returns>
        IList<Sys_Application> GetApp();
        /// <summary>
        /// 获得所有应用并分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        IList<Sys_Application> GetApp(int currentPage, int pageCount);
        /// <summary>
        /// 通过应用编号获取应用
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        string GetAppById(string applicationId);
        /// <summary>
        /// 获取应用编号最大值
        /// </summary>
        /// <returns></returns>
        int GetMaxApplicationId();
        /// <summary>
        /// 获取应用记录数
        /// </summary>
        /// <returns></returns>
        int GetAppCount();
    }
}
