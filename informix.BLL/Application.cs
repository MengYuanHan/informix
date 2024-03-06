using informix.IDAL;
using informix.Model.Entities;
using System.Collections.Generic;

namespace informix.BLL
{
    public class Application{
        private readonly IApplication _dal = DALFactory.DataAccess.CreateApplication();
        /// <summary>
        /// 获取所有应用并分页
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageCount">当前页显示数量</param>
        /// <returns></returns>
        public IList<Sys_Application> GetApp(int currentPage, int pageCount) => _dal.GetApp(currentPage, pageCount);
        /// <summary>
        /// 获取应用
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Application> GetApp() => _dal.GetApp();
        /// <summary>
        /// 根据编号获得应用
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public string GetAppById(string applicationId) => _dal.GetAppById(applicationId);
        /// <summary>
        /// 添加应用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddApp(Sys_Application application) => _dal.AddApp(application);
        /// <summary>
        /// 根据编号删除应用
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public bool DelAppById(string applicationId) => _dal.DelAppById(applicationId);
        /// <summary>
        /// 获取应用编号最大值
        /// </summary>
        /// <returns></returns>
        public int GetMaxApplicationId() => _dal.GetMaxApplicationId();
        /// <summary>
        /// 获取应用数量
        /// </summary>
        /// <returns></returns>
        public int GetAppCount() => _dal.GetAppCount();
    }
}
