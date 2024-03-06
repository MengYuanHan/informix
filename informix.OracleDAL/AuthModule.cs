using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.IDAL;
using informix.Model.Entities;
using log4net;
using NHibernate;
namespace informix.OracleDAL
{
    public class AuthModule:IAuthModule
    {
        private static ILog log = LogManager.GetLogger(typeof(AuthModule));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();
        protected ISession session { get; set; }
        public AuthModule()
        {
            this.session = nhibernateHelper.GetSession();
        }

        public AuthModule(ISession session)
        {
            this.session = session;
        }
        /// <summary>
        /// 添加模块权限
        /// </summary>
        /// <param name="obj">模块权限对象类</param>
        /// <returns></returns>
        public bool Add(Sys_Auth_Module authModule)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Auth_Module sam = new Sys_Auth_Module();
                sam.AuthModuleId = authModule.AuthModuleId;
                sam.AuthorityId = authModule.AuthorityId;
                sam.ModuleId = authModule.ModuleId;
                session.Save(sam);
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
        /// 根据模块编号删除模块权限
        /// </summary>
        /// <param name="moduleId">模块编号</param>
        /// <returns></returns>
        public bool DeleteAuthModule(int moduleId)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "Delete Sys_Auth_Module sam Where sam.ModuleId=:mi";
                session.CreateQuery(hql).SetInt32("mi", moduleId).ExecuteUpdate();
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
        /// 删除权限模块
        /// </summary>
        /// <param name="authorityId">权限编号</param>
        /// <returns></returns>
        public bool DelAuthModuleByAuthId(int authorityId)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "Delete Sys_Auth_Module sam Where sam.AuthorityId=:ai";
                session.CreateQuery(hql).SetInt32("ai", authorityId).ExecuteUpdate();
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
        /// 删除权限模块
        /// </summary>
        /// <param name="authorityId">权限编号</param>
        /// <param name="moduleId">模块编号</param>
        /// <returns></returns>
        public bool DelAuthModuleByModuleId(int authorityId, int moduleId)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "Delete Sys_Auth_Module sam Where sam.AuthorityId=:ai and sam.ModuleId=:mi";
                session.CreateQuery(hql).SetInt32("ai", authorityId).SetInt32("mi", moduleId).ExecuteUpdate();
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
    }
}
