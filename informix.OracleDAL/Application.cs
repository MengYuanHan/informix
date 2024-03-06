using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;
using informix.IDAL;
using informix.DBUtility;
using System.Configuration;
using NHibernate;
using System.Web.Script.Serialization;
using System.Collections;
using System.Data;
using log4net;

namespace informix.OracleDAL
{
    public class Application : IApplication
    {
        private static ILog log = LogManager.GetLogger(typeof(Application));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();

        protected ISession session { get; set; }

        public Application()
        {
            this.session = nhibernateHelper.GetSession();
        }

        public Application(ISession session)
        {
            this.session = session;
        }
        /// <summary>
        /// 查询所有应用并且分页
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Application> GetApp(int currentPage, int pageCount)
        {

            IList<Sys_Application> list = new List<Sys_Application>();

            int firstResult = 0;
            if (currentPage != 1)
            {
                firstResult = (currentPage - 1) * pageCount;
            }
            int lastResult = currentPage * pageCount;

            IQuery query = session.CreateQuery("from Sys_Application as sa order by sa.OrderBy")
                .SetFirstResult(firstResult).SetMaxResults(pageCount);
            list = query.List<Sys_Application>();

            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 查询所有应用
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Application> GetApp()
        {
            IList<Sys_Application> list = new List<Sys_Application>();
            IQuery query = session.CreateQuery("from Sys_Application as sa order by sa.OrderBy");
            list = query.List<Sys_Application>();
            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 根据应用编号查询应用
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public string GetAppById(string applicationId)
        {
            string json = null;
            string SQL_PARENTID = "SELECT moduleId FROM sys_module WHERE appId=" + applicationId + " AND parentId=0";
            OracleHelper.query(SQL_PARENTID);
            string sql = "SELECT t1.appname as appname,t2.moduleId as moduleId,t2.module as module,t2.directory as directory FROM sys_application t1,sys_module t2 WHERE t1.appId=t2.appId and t2.parentId ="+ OracleHelper.query(SQL_PARENTID).Tables[0].Rows[0]["MODULEID"].ToString() + "";
            DataTable dt = OracleHelper.query(sql).Tables[0];
            if (dt.Rows.Count == 0)
            {
                DataTable dtApp = OracleHelper.query("SELECT * FROM sys_module WHERE appId='" + applicationId + "' and parentId=0").Tables[0];
                json= Transform2JSON.ToJSONString(dtApp, dtApp.Rows.Count);
            }
            else
            {
                json = Transform2JSON.ToJSONString(dt, dt.Rows.Count);
            }
            
            return json;
        }
        /// <summary>
        /// 添加应用
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="decription"></param>
        /// <param name="url"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public bool AddApp(Sys_Application application)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Application sa = new Sys_Application();
                sa.AppName = application.AppName;
                sa.AppDesc = application.AppDesc;
                sa.Url = application.Url;
                sa.OrderBy = Convert.ToInt32(application.OrderBy);
                session.Save(sa);

                transaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result = false;
                //将异常写入到日志
                //LogHelper.WriteLog(ex);
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
        /// 删除应用
        /// </summary>
        /// <param name="applicationId">应用编号</param>
        /// <returns>返回结果字符串</returns>
        public bool DelAppById(string applicationId)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Application sa = new Sys_Application();
                sa.AppId = Convert.ToInt32(applicationId);
                session.Delete(sa);
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
        /// 获取最大应用编号
        /// </summary>
        /// <returns></returns>
        public int GetMaxApplicationId()
        {
            string hql = "SELECT max(AppId) as c FROM Sys_Application";

            ISQLQuery query = session.CreateSQLQuery(hql);
            return Convert.ToInt32(query.UniqueResult());
        }
        /// <summary>
        /// 获取应用记录数
        /// </summary>
        /// <returns>记录数</returns>
        public int GetAppCount()
        {
            int total = session.QueryOver<Sys_Application>().RowCount();
            return total;
        }
    }
}
