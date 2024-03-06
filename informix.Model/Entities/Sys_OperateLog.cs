using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_OperateLog {
        /// <summary>
        /// 操作日志编号
        /// </summary>
        public virtual int OperlogId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 写入时间
        /// </summary>
        public virtual DateTime WriteTime { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public virtual int UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        public virtual string IPAddress { get; set; }
        /// <summary>
        /// url
        /// </summary>
        public virtual string Url { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Contents { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public virtual string Others { get; set; }
    }
}
