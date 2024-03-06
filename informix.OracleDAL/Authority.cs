using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.DBUtility;
using System.Data;
using informix.IDAL;
using informix.Model.Entities;
using NHibernate;
using log4net;

namespace informix.OracleDAL
{
    public class Authority:IAuthority
    {
        private static ILog log = LogManager.GetLogger(typeof(Authority));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();

        protected ISession session { get; set; }

        public Authority()
        {
            this.session = nhibernateHelper.GetSession();
        }

        public Authority(ISession session)
        {
            this.session = session;
        }
        /// <summary>
        /// 当前用户是否拥有权限
        /// </summary>
        /// <returns></returns>
        public string GetAuthByUserId(string userId,string authorityId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT count(*) FROM sys_authority t1,sys_user_role t2,sys_auth_role t3 WHERE t1.authorityId=t3.authorityId and t2.roleId=t3.roleId and t2.userId='" + userId.Trim() + "' and t1.authorityId='" + authorityId.Trim() + "'");
            string result = OracleHelper.GetScalar(sb.ToString());

            return result;
        }
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddAuthority(Sys_Authority authority)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Authority sa = new Sys_Authority();
                sa.Authority = authority.Authority;
                sa.Type = authority.Type;
                sa.Remark = authority.Remark;

                session.Save(sa);

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
        /// 获取最大权限编号
        /// </summary>
        /// <returns></returns>
        public int GetMaxAuthorityId()
        {
            string hql = "SELECT max(AuthorityId) as AuthorityId FROM Sys_Authority sa";

            ISQLQuery query = session.CreateSQLQuery(hql);
            return Convert.ToInt32(query.UniqueResult());
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="authority">根据权限名</param>
        /// <returns></returns>
        public bool DeleteAuthority(string authority)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "Delete Sys_Authority sa Where sa.Authority=:ar";
                session.CreateQuery(hql).SetString("ar", authority).ExecuteUpdate();
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
        /// 获取权限编号
        /// </summary>
        /// <param name="authority">权限名</param>
        /// <returns></returns>
        public int GetAuthorityId(string authority) 
        {
            string hql = "SELECT AuthorityId FROM Sys_Authority sa WHERE sa.Authority=:ar";

            IQuery query = session.CreateQuery(hql).SetString("ar", authority);
            return Convert.ToInt32(query.UniqueResult());
        }
    }
}
