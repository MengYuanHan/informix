using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_Online {
        /// <summary>
        /// 在线编号
        /// </summary>
        public virtual int OnlineId { get; set; }
        /// <summary>
        /// GUID码[唯一]
        /// </summary>
        public virtual string GUID { get; set; }
        /// <summary>
        /// 登陆时间
        /// </summary>
        public virtual DateTime LoginTime { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        public virtual string IP { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public virtual int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// 是否可用[true/false]
        /// </summary>
        public virtual string isVailable { get; set; }
    }
}
