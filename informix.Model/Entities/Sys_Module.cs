using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_Module {
        /// <summary>
        /// 模块编号
        /// </summary>
        public virtual int ModuleId { get; set; }
        /// <summary>
        /// 应用编号
        /// </summary>
        public virtual int AppId { get; set; }
        /// <summary>
        /// 父节点编号
        /// </summary>
        public virtual int ParentId { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public virtual string Module { get; set; }
        /// <summary>
        /// 模块路径
        /// </summary>
        public virtual string Directory { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 是否为系统模块
        /// </summary>
        public virtual int Issystem { get; set; }
        /// <summary>
        /// 是否关闭
        /// </summary>
        public virtual int Close { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public virtual string Icon { get; set; }
        /// <summary>
        /// 活动状态
        /// </summary>
        public virtual string Status { get; set; }

        //要注意到一个Module是属于一个Application
        //public virtual Sys_Application Application { get; set; }

    }
}
