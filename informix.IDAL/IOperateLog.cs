using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;

namespace informix.IDAL {
    public interface IOperateLog {
        /// <summary>
        /// 创建操作日志记录
        /// </summary>
        /// <param name="operLog"></param>
        /// <returns></returns>
        void CreateOperLog(string title, string contents, string type, IList<Sys_User> user);
        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        IList<Sys_OperateLog> GetOperlog(int currentPage, int pageCount);
        /// <summary>
        /// 获取操作日志记录数
        /// </summary>
        /// <returns></returns>
        int GetOperlogCount();
    }
}
