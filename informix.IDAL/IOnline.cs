using informix.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.IDAL
{
    public interface IOnline
    {
        /// <summary>
        /// 生成在线唯一凭证[退出系统时删除、45分钟后自动删除]
        /// </summary>
        /// <param name="loginTime">登陆时间</param>
        /// <param name="userId">用户编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="isVailable">是否可用</param>
        bool Onlined(DateTime loginTime, string userId, string userName, string isVailable,string guid);
        /// <summary>
        /// 查询所有在线信息并分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        IList<Sys_Online> GetOnline(int currentPage, int pageCount);
        /// <summary>
        /// 获得在线数
        /// </summary>
        /// <returns></returns>
        int GetOnlineCount();
        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        bool Offline(string guid);
        /// <summary>
        /// 查询最近登录信息
        /// </summary>
        /// <returns></returns>
        IList<Sys_Online> GetOnline(string userId);
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="GUID"></param>
        /// <returns></returns>
        bool IsExist(string userName, string isVailable);

        void Update(string userName);
    }
}
