using informix.IDAL;
using informix.Model.Entities;
using System.Collections.Generic;

namespace informix.BLL
{
    public class OperateLog {
        private readonly IOperateLog _dal = DALFactory.DataAccess.CreateOperateLog();
        /// <summary>
        /// 生成操作日志
        /// </summary>
        /// <param name="operateLog"></param>
        public void CreateOperateLog(string title, string contents, string type, IList<Sys_User> user = null)
        {
            if (user == null)
            {
                //获取当前用户信息
                user = new User().GetCurrentUser();
            }

            _dal.CreateOperLog(title, contents, type, user);
        }
        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IList<Sys_OperateLog> GetOperlog(int currentPage, int pageCount) => _dal.GetOperlog(currentPage, pageCount);
        public int GetOperlogCount() => _dal.GetOperlogCount();
    }
}
