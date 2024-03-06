using informix.DBUtility;
using informix.IDAL;
using informix.Model.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Net;

namespace informix.OracleDAL
{
    public class Online:IOnline
    {
        private static ILog log = LogManager.GetLogger(typeof(Online));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();
        protected ISession session { get; set; }
        public Online()
        {
            this.session = nhibernateHelper.GetSession();
        }
        public Online(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// 生成在线唯一凭证[退出系统时删除、45分钟后自动删除]
        /// </summary>
        /// <param name="loginTime">登陆时间</param>
        /// <param name="userId">用户编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="isVailable">是否可用</param>
        /// <returns></returns>
        public bool Onlined(DateTime loginTime,string userId, string userName,string isVailable,string guid)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Online so = new Sys_Online();
                so.GUID = guid;
                so.LoginTime = loginTime;
                so.IP = GetAddressIP();
                so.UserId = Convert.ToInt32(userId);
                so.UserName = userName;
                so.isVailable = isVailable;

                session.Save(so);
                transaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result = false;
                //将异常写入到日志
                log.Error(ex);
            }
            finally
            {
                session.Flush();
                session.Close();
            }
            return result;
        }
        /// <summary>
        /// 查询所有在线用户
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IList<Sys_Online> GetOnline(int currentPage, int pageCount)
        {
            string hql = "FROM Sys_Online so where isVailable='true' order by so.OnlineId desc";
            IList<Sys_Online> list = new List<Sys_Online>();

            int firstResult = 0;
            if (currentPage != 1)
            {
                firstResult = (currentPage - 1) * pageCount + 1;
            }

            IQuery query = session.CreateQuery(hql).SetFirstResult(firstResult).SetMaxResults(pageCount);
            list = query.List<Sys_Online>();

            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 获取在线记录数
        /// </summary>
        /// <returns>记录数</returns>
        public int GetOnlineCount()
        {
            int total = session.QueryOver<Sys_Online>().Where(p=>p.isVailable=="true") .RowCount();
            return total;
        }
        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="IP">IP</param>
        /// <returns></returns>
        public bool Offline(string guid)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "UPDATE Sys_Online so SET isVailable='false' Where so.GUID='" + guid + "'";
                ISQLQuery query = session.CreateSQLQuery(hql).AddEntity(typeof(Sys_User));
                query.ExecuteUpdate();
                transaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result = false;
                //将异常写入到日志
                log.Error(ex);
            }
            finally
            {
                session.Flush();
                session.Close();
            }
            return result;
        }
        /// <summary>
        /// 查询最近登录信息
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Online> GetOnline(string userId)
        {
            string hql = "FROM Sys_Online WHERE OnlineId=(SELECT max(OnlineId) FROM Sys_Online WHERE UserId='" + userId + "')";
            IQuery query = session.CreateQuery(hql);
            IList<Sys_Online> list = new List<Sys_Online>();

            list = query.List<Sys_Online>();
            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        /// <returns></returns>
        public static string GetAddressIP()
        {
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
        public bool IsExist(string userName, string isVailable) {
            bool result = false;
            string hql = "FROM Sys_Online so WHERE so.UserName=:un and so.isVailable=:iv";
            IQuery query = session.CreateQuery(hql).SetString("un",userName).SetString("iv",isVailable);
            IList<Sys_Online> list = query.List<Sys_Online>();
            if (list.Count > 0)
            {
                result = true;
            }
            return result;
        }
        public void Update(string userName) {
            string SQL_UPDATE = "UPDATE Sys_Online SET isVailable='false' WHERE UserName='"+userName+ "' AND  isVailable='true'";
            int count = OracleHelper.ExecuteSQL(SQL_UPDATE);
        }
    }
}
