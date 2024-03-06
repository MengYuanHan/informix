using informix.IDAL;
using informix.Model.Entities;
using System.Collections.Generic;

namespace informix.BLL
{
    public class Department{
        private readonly IDepartment _dal = DALFactory.DataAccess.CreateDepartment();
        /// <summary>
        /// 通过deptId获取组
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public IList<Sys_Department> GetGroupByDetpId(string deptId) => _dal.GetGroupByDetpId(deptId);
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Department> GetDepartment() => _dal.GetDepartment();
        /// <summary>
        /// 获取部门记录数
        /// </summary>
        /// <returns></returns>
        public int GetDeptCount() => _dal.GetDeptCount();
        /// <summary>
        /// 获取部门并分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IList<Sys_Department> GetDepartment(int currentPage, int pageCount) => _dal.GetDepartment(currentPage, pageCount);
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddDept(Sys_Department department) => _dal.AddDept(department);
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public bool deleteDept(string deptId) => _dal.deleteDept(deptId);
    }
}
