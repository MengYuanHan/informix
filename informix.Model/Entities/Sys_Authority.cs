using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_Authority {
        /// <summary>
        /// 权限编号
        /// </summary>
        public virtual int AuthorityId { get; set; }
        /// <summary>
        /// 权限名
        /// </summary>
        public virtual string Authority { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
