using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_Auth_Role {
        /// <summary>
        /// 角色权限编号
        /// </summary>
        public virtual int AuthRoleId { get; set; }
        /// <summary>
        /// 权限编号
        /// </summary>
        public virtual int AuthorityId { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public virtual int RoleId { get; set; }

    }
}
