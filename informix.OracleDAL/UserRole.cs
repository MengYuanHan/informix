using informix.DBUtility;
using informix.IDAL;
using informix.Model.Entities;
using log4net;
using NHibernate;
using System;
using System.Text;

namespace informix.OracleDAL
{
    public class UserRole:IUserRole
    {
        private static ILog log = LogManager.GetLogger(typeof(UserRole));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();
        protected ISession session { get; set; }
        public UserRole()
        {
            this.session = nhibernateHelper.GetSession();
        }
        public UserRole(ISession session)
        {
            this.session = session;
        }
        /// <summary>
        /// 添加用户角色
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddUserRole(Sys_User_Role userRole)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_User_Role sur = new Sys_User_Role();
                sur.RoleId = userRole.RoleId;
                sur.UserId = userRole.UserId;
                sur.Descript = userRole.Descript;
                session.Save(sur);
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
        /// 删除用户角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool DelUserRole(string roleId)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "Delete Sys_User_Role sur Where sur.RoleId=:ri";
                session.CreateQuery(hql).SetInt32("ri", Convert.ToInt32(roleId)).ExecuteUpdate();
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
        /// 删除用户角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public bool DelUserRoleByUR(string userId, string roleId)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "Delete Sys_User_Role sur WHERE sur.RoleId=:ri AND sur.UserId=:ui";
                session.CreateQuery(hql).SetInt32("ri", Convert.ToInt32(roleId)).SetInt32("ui", Convert.ToInt32(userId)).ExecuteUpdate();
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
        /// 判断用户是否拥有权限
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public int IsUserRole(string userId, string roleId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT count(*) FROM sys_user_role WHERE UserId='" + userId + "' and RoleId='" + roleId + "'");

            int count = Convert.ToInt32(OracleHelper.GetScalar(sb.ToString()));
            return count;
        }
    }
}
