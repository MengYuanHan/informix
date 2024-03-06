using informix.DBUtility;
using informix.IDAL;
using informix.Model.Entities;
using log4net;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace informix.OracleDAL
{
    public class User : IUser
    {
        private static ILog log = LogManager.GetLogger(typeof(User));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();
        public ResponseHelper rh = new ResponseHelper();
        protected ISession session { get; set; }
        public User()
        {
            this.session = nhibernateHelper.GetSession();
        }
        public User(ISession session)
        {
            this.session = session;
        }
        
        /// <summary>
        /// 验证登录名、密码
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="loginPassword">密码</param>
        /// <returns>返回记录数</returns>
        public decimal ValidateUserInfo(string userName, string userPwd)
        {
            userPwd = DESHelper.Encrypt(userPwd);
            IQuery query = session.CreateQuery("FROM Sys_User c WHERE c.UserName=:un and c.Password=:up").SetString("un", userName).SetString("up", userPwd);

            return query.List<Sys_User>().Count();
        }
        /// <summary>
        /// 查询当前用户是否停用
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string IsEnable(string userId)
        {
            string sql = "SELECT State FROM Sys_User WHERE UserId='" + userId + "'";

            return OracleHelper.GetScalar(sql);
        }
        /// <summary>
        /// 根据用户名与密码返回用户编号
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <param name="loginPassword">用户密码</param>
        /// <returns></returns>
        public int GetUserId(string userName, string userPwd)
        {
            userPwd = DESHelper.Encrypt(userPwd);
            string query = "SELECT UserId FROM Sys_User WHERE UserName='" + userName + "' and Password='" + userPwd + "'";
            return Convert.ToInt32(OracleHelper.GetScalar(query));
        }
        /// <summary>
        /// 根据用户名和密码获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <returns></returns>
        public SortedDictionary<string, string> GetUserInfoByUserNameAndPwd(string userName, string userPwd) {
            //将密码加密
            userPwd = DESHelper.Encrypt(userPwd);
            //查询对应数据
            IList<Sys_User> user = session.QueryOver<Sys_User>().Where(p => p.UserName == userName).And(p => p.Password == userPwd).List();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //序列化
            string json = serializer.Serialize(user);
            rh.m_values.Clear();
            //反序列化
            rh.m_values = rh.jsonToObject(json);
            return rh.m_values;
        }
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="currentPage">当前页码</param>
        /// <param name="pageCount">当前页显示数量</param>
        /// <returns></returns>
        public IList<Sys_User> GetUser(int currentPage, int pageCount){
            string hql = "FROM Sys_User s order by s.UserId asc";
            IList<Sys_User> list = new List<Sys_User>();
            int firstResult = 0;
            IQuery query = session.CreateQuery(hql).SetFirstResult(firstResult).SetMaxResults(pageCount);
            list = query.List<Sys_User>();

            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public string UpdateState(string userId, string state)
        {
            string result = null;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                string hql = "UPDATE Sys_User SET State='" + state + "' WHERE UserId='" + userId + "'";
                ISQLQuery query = session.CreateSQLQuery(hql).AddEntity(typeof(Sys_User));
                query.ExecuteUpdate();

                transaction.Commit();
                result = "1";
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
        /// 获取用户记录数
        /// </summary>
        /// <returns>记录数</returns>
        public int GetUserCount()
        {
            int total = session.QueryOver<Sys_User>().RowCount();
            return total;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="obj">用户列表集合</param>
        /// <returns></returns>
        public bool AddUser(Sys_User user)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_User su = new Sys_User();
                su.UserName = user.UserName;
                su.Password = DESHelper.Encrypt(user.Password);
                su.DeptId = user.DeptId;
                su.Sex = user.Sex;
                su.RealName = user.RealName;
                su.Title = user.Title;
                su.Phone = user.Phone;
                su.Fax = user.Fax;
                su.Email = user.Email;
                su.Province = user.Province;
                su.City = user.City;
                su.Address = user.Address;
                su.QQ = user.QQ;
                su.NickName = user.NickName;
                su.CreateUserId = user.CreateUserId;
                su.CreateDate = user.CreateDate;
                su.State = user.State;
                session.Save(su);
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
        /// 获取最大用户编号
        /// </summary>
        /// <returns></returns>
        public int GetMaxUserId()
        {
            string hql = "SELECT max(UserId) as UserId FROM Sys_User su";

            ISQLQuery query = session.CreateSQLQuery(hql);
            return Convert.ToInt32(query.UniqueResult());
        }
        /// <summary>
        /// 根据UserId获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Sys_User> GetUserByUserId(int userId)
        {
            string hql = "FROM Sys_User s WHERE s.UserId=:ui order by s.UserId asc";
            IList<Sys_User> list = new List<Sys_User>();

            IQuery query = session.CreateQuery(hql).SetInt32("ui", userId);
            list = query.List<Sys_User>();

            for (int i = 0; i < list.Count; i++) {
                list[i].Password = DESHelper.Decrypt(list[i].Password);
            }
            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool UpdateUser(Sys_User user)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_User su = new Sys_User();
                su.UserId = user.UserId;
                su.UserName = user.UserName;
                su.Password = DESHelper.Encrypt(user.Password);
                su.DeptId = user.DeptId;
                su.Sex = user.Sex;
                su.RealName = user.RealName;
                su.Title = user.Title;
                su.Phone = user.Phone;
                su.Fax = user.Fax;
                su.Email = user.Email;
                su.Province = user.Province;
                su.City = user.City;
                su.Address = user.Address;
                su.QQ = user.QQ;
                su.NickName = user.NickName;
                su.CreateUserId = user.CreateUserId;
                su.CreateDate = user.CreateDate;
                su.State = user.State;
                session.Merge(su);
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
                //session.Close();
            }
            return result;
        }
        /// <summary>
        /// 根据userId判断用户是否拥有修改其他用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsVerifyAuthority(string userId)
        {
            bool result = false;
            string SQL_VERIFY_AUTHORITY = "SELECT count(*) FROM sys_user_role t1,sys_auth_role t2,sys_authority t3 WHERE t1.userId='" + userId + "' and t1.roleId=T2.ROLEID and t2.authorityId=t3.authorityId and t3.authority='修改用户'";

            int count = Convert.ToInt32(OracleHelper.GetScalar(SQL_VERIFY_AUTHORITY));

            if (count == 1)
            {
                result = true;
            }

            return result;
        }
        /// <summary>
        /// 获取当前用户UserId
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserId()
        {
            return new Linq2xmlHelper().GetChildElValueByElIdValue("session", "temp", "UserId");
        }
        /// <summary>
        /// 根据登录名和密码查出用户信息
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="loginPassword">密码</param>
        /// <returns></returns>
        public IList<Sys_User> GetUserInfo(string userName, string userPwd)
        {
            string hql = "FROM Sys_User s WHERE s.UserName=:un and s.Password=:up order by s.UserId asc";
            IList<Sys_User> list = new List<Sys_User>();
            userPwd = DESHelper.Encrypt(userPwd);
            IQuery query = session.CreateQuery(hql).SetString("un", userName).SetString("up", userPwd);
            list = query.List<Sys_User>();

            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public IList<Sys_User> GetCurrentUser() {
            string hql = "FROM Sys_User s WHERE s.UserId=:li order by s.UserId asc";

            IList<Sys_User> list = new List<Sys_User>();
            IQuery query = session.CreateQuery(hql).SetString("li", GetCurrentUserId());
            list = query.List<Sys_User>();

            return list.Count > 0 ? list : null;
        }
    }
}
