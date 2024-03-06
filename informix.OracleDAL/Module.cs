using informix.DBUtility;
using informix.IDAL;
using informix.Model.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace informix.OracleDAL
{
    public class Module : IModule
    {
        private static ILog log = LogManager.GetLogger(typeof(Module));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();

        protected ISession session { get; set; }

        public Module()
        {
            this.session = nhibernateHelper.GetSession();
        }

        public Module(ISession session)
        {
            this.session = session;
        }
        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <returns>返回模块列表字符串</returns>
        public string GetModuleList()
        {
            //实例化xml
            string userId =new Linq2xmlHelper().GetChildElValueByElIdValue("session", "temp", "UserId");
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT distinct t1.moduleid,t1.parentid,t1.module,t1.directory,t1.code,t1.issystem,t1.close,t1.icon,t1.status,level FROM ");
            sb.Append("sys_module t1,sys_user t2,sys_user_role t3, sys_role t4,sys_auth_role t5,sys_authority t6 ,sys_auth_module t7 ");
            sb.Append("WHERE t1.moduleid=t7.moduleid and t2.userid = t3.userid and t3.roleid = t4.roleid and t4.roleid=t5.roleid and t5.authorityid=t6.authorityid and t6.authorityid=t7.authorityid ");
            sb.Append("and t2.userid = '" + userId + "' and t1.close = 0 start with t1.parentid = 0 connect by prior t1.moduleid = t1.parentid order by code asc");

            DataTable dt = OracleHelper.query(sb.ToString()).Tables[0];
            string json = Transform2JSON.ToJSONString(dt, dt.Rows.Count);

            return json;
        }
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="obj">模块实体类</param>
        /// <returns></returns>
        public bool AddAppModule(Sys_Module module)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Module sm = new Sys_Module();
                sm.AppId = module.AppId;
                sm.ParentId = module.ParentId;
                sm.Module = module.Module;
                if (string.IsNullOrEmpty(module.Directory))
                {
                    sm.Directory = "/Home/index";
                }
                else
                {
                    sm.Directory = module.Directory;
                }
                
                sm.Code = module.Code;
                sm.Issystem = module.Issystem;
                sm.Close = module.Close;
                sm.Icon = module.Icon;
                sm.Status = module.Status;

                session.Save(sm);

                transaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result = false;
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
        /// 自动获取模块代码
        /// </summary>
        /// <returns></returns>
        public string GetCode()
        {
            string hql = "SELECT Code+1000 FROM sys_module WHERE moduleId=(SELECT max(moduleId) FROM sys_module) and parentId='0'";
            ISQLQuery query = session.CreateSQLQuery(hql);

            return Convert.ToString(query.UniqueResult());
        }
        /// <summary>
        /// 获得模块Id最大值
        /// </summary>
        /// <returns></returns>
        public int GetMaxModuleId()
        {
            string hql = "SELECT max(ModuleId) as c FROM Sys_Module";
            ISQLQuery query = session.CreateSQLQuery(hql);
            return Convert.ToInt32(query.UniqueResult());
        }
        /// <summary>
        /// 通过应用Id获得模块Id
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public int GetModuleIdByAppId(string applicationId)
        {
            string hql = "SELECT moduleid FROM sys_module WHERE appId='" + applicationId + "'";
            ISQLQuery query = session.CreateSQLQuery(hql);
            return Convert.ToInt32(query.UniqueResult());
        }
        /// <summary>
        /// 通过应用Id删除模块
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public bool DelByAppId(string applicationId)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "Delete Sys_Module sm Where sm.AppId=:ai";
                session.CreateQuery(hql).SetString("ai", applicationId).ExecuteUpdate();
                transaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result = false;
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
        /// 获取所有模块并且按模块代码排序
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Module> GetModule()
        {
            string hql = "SELECT s FROM Sys_Module s where s.ParentId<>0 order by s.Code asc";
            IList<Sys_Module> list = new List<Sys_Module>();
            IQuery query = session.CreateQuery(hql);
            list = query.List<Sys_Module>();
            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 通过角色Id获取权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetAuthByRoleId(string roleId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM sys_auth_module t1,sys_auth_role t2, sys_role t3 ");
            sb.Append("WHERE t1.authorityId=t2.authorityId and t2.roleId=t3.roleId and t3.roleId='" + roleId + "' order by t1.moduleId asc");
            DataTable dt = OracleHelper.query(sb.ToString()).Tables[0];
            string json = Transform2JSON.ToJSONString(dt, dt.Rows.Count);
            return json;
        }
        /// <summary>
        /// 通过应用Id查询模块
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <returns></returns>
        public IList<Sys_Module> GetAppModuleByAppId(string appId)
        {
            IQuery query = session.CreateQuery("from Sys_Module c where c.AppId=:ai and c.ParentId=0").SetInt32("ai", Convert.ToInt32(appId));
            return query.List<Sys_Module>();
        }        
        /// <summary>
        /// 获取应用模块数
        /// </summary>
        /// <returns></returns>
        public int GetAppModuleCount()
        {
            int total = session.QueryOver<Sys_Module>().RowCount();
            return total;
        }
        /// <summary>
        /// 更新模块状态
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="close"></param>
        /// <returns></returns>
        public string UpdateAppModuleStatus(string moduleId, string close)
        {
            string result = null;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "UPDATE Sys_Module SET Close='" + close + "' WHERE moduleId='" + moduleId + "'";
                ISQLQuery query = session.CreateSQLQuery(hql).AddEntity(typeof(Sys_User));
                query.ExecuteUpdate();
                transaction.Commit();
                result = close;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
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
        /// 根据模块Id删除模块
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public bool DeleteAppModuleByModuleId(string moduleId)
        {
            bool result = false;
            ITransaction transaction = session.BeginTransaction();

            try
            {
                string hql = "Delete Sys_Module sm Where sm.ModuleId=:mi";
                session.CreateQuery(hql).SetString("mi", moduleId).ExecuteUpdate();
                transaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result = false;
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
        /// 根据模块Id查询模块信息
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public DataTable GetAppModuleById(string moduleId)
        {
            string SQL_MODULE = "SELECT t1.appid,t1.appname,t2.*,(SELECT moduleid FROM sys_module WHERE moduleid=(SELECT parentid FROM sys_module WHERE moduleid=t2.moduleid)) p_moduleid,(SELECT module FROM sys_module WHERE moduleid = (SELECT parentid FROM sys_module WHERE moduleid = t2.moduleid)) p_module FROM sys_application t1, sys_module t2 WHERE t1.appid = t2.appid and t2.moduleid =" + moduleId;
            DataTable dt = OracleHelper.query(SQL_MODULE).Tables[0];
            return dt;
        }
        /// <summary>
        /// 更新应用模块
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool UpdateAppModule(Sys_Module obj)
        {
            bool result = false;
            ITransaction transaction = session.BeginTransaction();

            try
            {
                Sys_Module sm = new Sys_Module();

                sm.ModuleId = obj.ModuleId;
                sm.AppId = obj.AppId;
                sm.ParentId = obj.ParentId;
                sm.Module = obj.Module;
                sm.Directory = obj.Directory;
                sm.Code = obj.Code;
                sm.Issystem = obj.Issystem;
                sm.Close = obj.Close;
                sm.Icon = obj.Icon;
                sm.Status = obj.Status;

                session.Update(sm);
                transaction.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                result = false;
                log.Error(ex);
            }
            finally
            {
                session.Flush();
                session.Close();
            }
            return result;
        }
    }
}
