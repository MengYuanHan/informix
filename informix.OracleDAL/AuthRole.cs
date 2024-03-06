using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.IDAL;
using informix.Model.Entities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Cfg;
using informix.DBUtility;
using log4net;

namespace informix.OracleDAL
{
    public class AuthRole:IAuthRole
    {
        private static ILog log = LogManager.GetLogger(typeof(AuthRole));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();
        protected ISession session { get; set; }
        public AuthRole()
        {
            this.session = nhibernateHelper.GetSession();
        }
        public AuthRole(ISession session)
        {
            this.session = session;
        }
        /// <summary>
        /// 删除角色权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public bool DelAuthRole(string roleId)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "Delete Sys_Auth_Role sar Where sar.RoleId=:ri";
                session.CreateQuery(hql).SetInt32("ri",Convert.ToInt32(roleId)).ExecuteUpdate();
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
        /// 判断是否存在次角色权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="moduleId">模块编号</param>
        /// <returns></returns>
        public int IsAuthRole(string roleId,string moduleId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT count(*) FROM sys_auth_module t1,sys_auth_role t2 WHERE t1.authorityId=t2.authorityId and t2.roleId='" + roleId + "' and t1.moduleId='" + moduleId + "'");

           int count=Convert.ToInt32(OracleHelper.GetScalar(sb.ToString()));
            return count;
        }
        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="authRole"></param>
        /// <returns></returns>
        public bool AddAuthRole(Sys_Auth_Role authRole)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Auth_Role sar = new Sys_Auth_Role();
                sar.AuthorityId = authRole.AuthorityId;
                sar.RoleId = authRole.RoleId;

                session.Save(sar);

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
        /// 根据角色编号获取权限编号
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public int GetAuthorityIdByRoleId(int roleId)
        {
            string hql = "SELECT AuthorityId FROM Sys_Auth_Role sar WHERE sar.RoleId=:ri order by AuthorityId";

            IQuery query = session.CreateQuery(hql).SetInt32("ri", roleId).SetMaxResults(1);
            return Convert.ToInt32(query.UniqueResult());
        }
    }
}
