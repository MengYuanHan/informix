using informix.IDAL;
using informix.Model.Entities;
using System;
using System.Collections.Generic;

namespace informix.BLL
{
    public class Online{
        private readonly IOnline _dal = DALFactory.DataAccess.CreateOnline();
        /// <summary>
        /// 生成在线唯一凭证[退出系统时删除、45分钟后自动删除]
        /// </summary>
        /// <param name="loginTime">登陆时间</param>
        /// <param name="userId">用户编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="isVailable">是否可用</param>
        public bool Onlined(DateTime loginTime, string userId, string userName, string isVailable, string guid) => _dal.Onlined(loginTime, userId, userName, isVailable, guid);
        /// <summary>
        /// 获取在线信息并分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IList<Sys_Online> GetOnline(int currentPage, int pageCount) => _dal.GetOnline(currentPage, pageCount);
        /// <summary>
        /// 获取在线数
        /// </summary>
        /// <returns></returns>
        public int GetOnlineCount() => _dal.GetOnlineCount();
        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool Offline(string guid) => _dal.Offline(guid);
        /// <summary>
        /// 查询最近登录信息
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Online> GetOnline(string userId) => _dal.GetOnline(userId);
        public bool IsExist(string userName, string isVailable) => _dal.IsExist(userName, isVailable);
        public void Update(string userName) => _dal.Update(userName);
    }
}
