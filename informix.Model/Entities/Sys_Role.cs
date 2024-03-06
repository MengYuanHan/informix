using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_Role {
        /// <summary>
        /// 角色编号
        /// </summary>
        public virtual int RoleId { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual string Role { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        public virtual string RDesc { get; set; }
    }
}
