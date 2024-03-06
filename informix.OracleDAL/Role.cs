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
    public class Role:IRole
    {
        private static ILog log = LogManager.GetLogger(typeof(Role));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();
        protected ISession session { get; set; }
        public Role()
        {
            this.session = nhibernateHelper.GetSession();
        }
        public Role(ISession session)
        {
            this.session = session;
        }
        /// <summary>
        /// 获取全部角色权限
        /// </summary>
        /// <returns></returns>
        public string GetRoleAuth()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT t1.roleId,t1.role,t3.authorityId,t3.authority,t3.type,t5.moduleId,t5.parentId,t5.module,t5.directory,t5.code FROM sys_role t1,sys_auth_role t2,sys_authority t3,sys_auth_module t4,sys_module t5 ");
            sb.Append("WHERE t1.roleId = t2.roleId and t2.authorityId = t3.authorityId and t3.authorityId = t4.authorityId and t4.moduleId = t5.moduleId order by t5.moduleId asc");
            DataTable dt = OracleHelper.query(sb.ToString()).Tables[0];
            string json = Transform2JSON.ToJSONString(dt, dt.Rows.Count);
            return json;
        }
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageCount">分页数量</param>
        /// <returns></returns>
        public IList<Sys_Role> GetRole(int currentPage, int pageCount)
        {
            string hql = "FROM Sys_Role r order by r.RoleId asc";
            IList<Sys_Role> list = new List<Sys_Role>();

            int firstResult = 0;
            if (currentPage != 1)
            {
                firstResult = (currentPage - 1) * pageCount;
            }
            int lastResult = currentPage * pageCount;

            IQuery query = session.CreateQuery(hql).SetFirstResult(firstResult).SetMaxResults(pageCount);
            list = query.List<Sys_Role>();

            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 获取角色记录数
        /// </summary>
        /// <returns>记录数</returns>
        public int GetRoleCount()
        {
            int total = session.QueryOver<Sys_Role>().RowCount();
            return total;
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="obj">角色列表</param>
        /// <returns></returns>
        public bool AddRole(Sys_Role role)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Role sr = new Sys_Role();
                sr.Role = role.Role;
                sr.RDesc = role.RDesc;
                session.Save(sr);
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
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public bool DelRole(string roleId)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Role sr = new Sys_Role();
                sr.RoleId = Convert.ToInt32(roleId);
                session.Delete(sr);
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
        /// 获取最大角色编号
        /// </summary>
        /// <returns></returns>
        public int GetMaxRoleId()
        {
            string hql = "SELECT max(RoleId) as RoleId FROM Sys_Role sr";

            ISQLQuery query = session.CreateSQLQuery(hql);
            return Convert.ToInt32(query.UniqueResult());
        }
        /// <summary>
        /// 按角色编号获取角色
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Role> GetRole()
        {
            string hql = "SELECT r FROM Sys_Role r order by r.RoleId asc";
            IList<Sys_Role> list = new List<Sys_Role>();

            IQuery query = session.CreateQuery(hql);
            list = query.List<Sys_Role>();

            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 根据用户编号获取剩余角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public string GetSurplusRole(string userId)
        {
            string json = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM sys_role WHERE roleId NOT IN(SELECT roleId FROM sys_user_role WHERE UserId='"+userId+ "') ORDER BY roleId");
            DataTable dt = OracleHelper.query(sb.ToString()).Tables[0];
            json = Transform2JSON.ToJSONString(dt, dt.Rows.Count);
            return json;
        }
        /// <summary>
        /// 根据角色编号所拥有角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public string GetCurrentUserRole(string userId)
        {
            string json = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT t1.* FROM sys_role t1,sys_user_role t2 WHERE t1.roleId=t2.roleId and UserId='" + userId + "' ORDER BY t1.roleId");
            DataTable dt = OracleHelper.query(sb.ToString()).Tables[0];
            json = Transform2JSON.ToJSONString(dt, dt.Rows.Count);
            return json;
        }
        /// <summary>
        /// 根据角色编号获取角色
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public string GetRoleByRoleId(int roleId)
        {
            IQuery query = session.CreateQuery("from Sys_Role sr where sr.RoleId=:ri").SetInt32("ri", roleId);
            return query.List<Sys_Role>()[0].Role;
        }
    }
}
