using informix.IDAL;
using informix.Model.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace informix.OracleDAL
{
    public class OperateLog : IOperateLog {
        private static ILog log = LogManager.GetLogger(typeof(OperateLog));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();
        protected ISession session { get; set; }
        public OperateLog()
        {
            this.session = nhibernateHelper.GetSession();
        }
        public OperateLog(ISession session)
        {
            this.session = session;
        }
        /// <summary>
        /// 创建操作日志记录
        /// </summary>
        /// <param name="operLog"></param>
        /// <returns></returns>
        public void CreateOperLog(string title, string contents, string type, IList<Sys_User> user) {
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_OperateLog so = new Sys_OperateLog();
                so.Title = title;
                so.Type = type;
                so.WriteTime = System.DateTime.Now;
                for (int i = 0; i < user.Count; i++)
                {
                    so.UserId = user[i].UserId;
                    so.UserName = user[i].UserName;
                }
                
                so.IPAddress = GetAddressIP();
                so.Url = HttpContext.Current.Request.Url.ToString();
                so.Contents = contents;
                so.Others = string.Format("操作系统：{0} 浏览器：{1}", GetOSName(), GetBrowse());
                
                session.Save(so);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                log.Error(ex);
            }
            finally
            {
                session.Flush();
                session.Close();
            }
        }
        /// <summary>
        /// 获取浏览器端操作系统名称
        /// </summary>
        /// <returns></returns>
        public static string GetOSName()
        {
            string osVersion = System.Web.HttpContext.Current.Request.Browser.Platform;
            string userAgent = System.Web.HttpContext.Current.Request.UserAgent;

            if (userAgent.Contains("NT 6.3"))
            {
                osVersion = "Windows8.1";
            }
            else if (userAgent.Contains("NT 6.2"))
            {
                osVersion = "Windows8";
            }
            else if (userAgent.Contains("NT 6.1"))
            {
                osVersion = "Windows7";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                osVersion = "WindowsVista";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                osVersion = "WindowsServer2003";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osVersion = "WindowsXP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osVersion = "Windows2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osVersion = "WindowsNT4.0";
            }
            else if (userAgent.Contains("Me"))
            {
                osVersion = "WindowsMe";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }
            return osVersion;
        }

        /// <summary>
        /// 得到用户浏览器类型
        /// </summary>
        /// <returns></returns>
        public static string GetBrowse()
        {
            return System.Web.HttpContext.Current.Request.Browser.Type;
        }
        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        /// <returns></returns>
        public static string GetAddressIP(){
            string IP = null;
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            IP = AddressIP;
            return IP;
        }
        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IList<Sys_OperateLog> GetOperlog(int currentPage, int pageCount) {
            string hql = "FROM Sys_OperateLog s order by s.WriteTime DESC";
            IList<Sys_OperateLog> operlog = new List<Sys_OperateLog>();
            int firstResult = 0;
            if (currentPage != 1)
            {
                firstResult = (currentPage - 1) * pageCount + 1;
            }
            operlog = session.CreateQuery(hql).SetFirstResult(firstResult).SetMaxResults(pageCount).List<Sys_OperateLog>();
            return operlog.Count > 0 ? operlog : null;
        }
        /// <summary>
        /// 获取操作记录数
        /// </summary>
        /// <returns>记录数</returns>
        public int GetOperlogCount()
        {
            int total = session.QueryOver<Sys_OperateLog>().RowCount();
            return total;
        }
    }
}
