using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.Model.Entities;

namespace informix.IDAL
{
    public interface IDepartment
    {
        /// <summary>
        /// 通过deptId查询组
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        IList<Sys_Department> GetGroupByDetpId(string deptId);
        /// <summary>
        /// 查询所有部门
        /// </summary>
        /// <returns></returns>
        IList<Sys_Department> GetDepartment();
        /// <summary>
        /// 查询部门数
        /// </summary>
        /// <returns></returns>
        int GetDeptCount();
        /// <summary>
        /// 查询所有部门并分页
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        IList<Sys_Department> GetDepartment(int currentPage, int pageCount);
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool AddDept(Sys_Department department);
        /// <summary>
        /// 获得最大值
        /// </summary>
        /// <returns></returns>
        int GetMaxShowOrder();
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        bool deleteDept(string deptId);
    }
}
