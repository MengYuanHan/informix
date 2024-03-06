using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_Department {
        /// <summary>
        /// 部门编号
        /// </summary>
        public virtual int DeptId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public virtual string Dept { get; set; }
        /// <summary>
        /// 父编号
        /// </summary>
        public virtual int ParentId { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public virtual int ShowOrder { get; set; }
        /// <summary>
        /// 部门级别
        /// </summary>
        public virtual int DeptLevel { get; set; }
        /// <summary>
        /// 子节点数
        /// </summary>
        public virtual int ChildCount { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual int Deletes { get; set; }
    }
}
