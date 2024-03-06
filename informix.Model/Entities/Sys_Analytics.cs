using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_Analytics {
        /// <summary>
        /// 分析日志编号
        /// </summary>
        public virtual int AnalyticsId { get; set; }
        /// <summary>
        /// 日志文件名
        /// </summary>
        public virtual int FileName { get; set; }
        /// <summary>
        /// 日志文件类型
        /// </summary>
        public virtual int FileType { get; set; }
        /// <summary>
        /// 完整路径
        /// </summary>
        public virtual int WholeUrl { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public virtual int FileSize { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual int CreateTime { get; set; }
        /// <summary>
        /// 写入时间
        /// </summary>
        public virtual int WriteTime { get; set; }
        /// <summary>
        /// 访问时间
        /// </summary>
        public virtual int AccessTime { get; set; }
    }
}
