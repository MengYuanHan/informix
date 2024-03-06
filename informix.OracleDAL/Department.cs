using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.IDAL;
using informix.Model.Entities;
using log4net;
using NHibernate;
//using informix.OracleDAL.NHibernateHelpers;

namespace informix.OracleDAL
{
    public class Department:IDepartment
    {
        private static ILog log = LogManager.GetLogger(typeof(Department));
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();
        protected ISession session { get; set; }
        public Department()
        {
            this.session = nhibernateHelper.GetSession();
        }
        public Department(ISession session)
        {
            this.session = session;
        }
        /// <summary>
        /// 查询部门和组
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Department> GetWholeQuery() {
            string hql = "FROM Sys_Department";
            IList<Sys_Department> list = new List<Sys_Department>();

            IQuery query = session.CreateQuery(hql);
            list = query.List<Sys_Department>();

            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 通过deptId查询组
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public IList<Sys_Department> GetGroupByDetpId(string  deptId)
        {
            var query = from c in GetWholeQuery()
                        where c.DeptId ==Convert.ToInt32(deptId)
                        select c;
            return query.ToList<Sys_Department>();
        }
        /// <summary>
        /// 获取部门信息[用户资料添加和修改]
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Department> GetDepartment()
        {
            string hql = "FROM Sys_Department sd WHERE ParentId=0 order by sd.ShowOrder asc";
            IList<Sys_Department> list = new List<Sys_Department>();

            IQuery query = session.CreateQuery(hql);
            list = query.List<Sys_Department>();

            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 获取部门记录数
        /// </summary>
        /// <returns>记录数</returns>
        public int GetDeptCount()
        {
            int total = session.QueryOver<Sys_Department>().RowCount();
            return total;
        }
        /// <summary>
        /// 获得部门记录并且分页
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageCount">页面数量</param>
        /// <returns></returns>
        public IList<Sys_Department> GetDepartment(int currentPage, int pageCount)
        {
            string hql = "FROM Sys_Department sd  WHERE ParentId=0 order by sd.ShowOrder asc";
            IList<Sys_Department> list = new List<Sys_Department>();
            int firstResult = 0;
            if (currentPage != 1)
            {
                firstResult = (currentPage - 1) * pageCount;
            }
            int lastResult = currentPage * pageCount;
            IQuery query = session.CreateQuery(hql).SetFirstResult(firstResult).SetMaxResults(pageCount);
            list = query.List<Sys_Department>();

            return list.Count > 0 ? list : null;
        }
        /// <summary>
        /// 获取排序最大值
        /// </summary>
        /// <returns></returns>
        public int GetMaxShowOrder()
        {
            string hql = "SELECT max(ShowOrder) as ShowOrder FROM Sys_Department sd";

            ISQLQuery query = session.CreateSQLQuery(hql);
            return Convert.ToInt32(query.UniqueResult());
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddDept(Sys_Department department)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Department sd = new Sys_Department();
                sd.Dept = department.Dept;
                sd.ParentId = 0;
                sd.ShowOrder = GetMaxShowOrder();
                sd.DeptLevel = department.DeptLevel;
                sd.ChildCount = department.ChildCount;
                sd.Deletes = 0;
                session.Save(sd);
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
        /// 根绝部门编号删除部门
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public bool deleteDept(string deptId)
        {
            bool result;
            ITransaction transaction = session.BeginTransaction();
            try
            {
                Sys_Department sd = new Sys_Department();
                sd.DeptId = Convert.ToInt32(deptId);
                session.Delete(sd);
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
