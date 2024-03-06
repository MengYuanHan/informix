using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_User_Role {
        /// <summary>
        /// 用户角色编号
        /// </summary>
        public virtual int UserRoleId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public virtual int UserId { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public virtual int RoleId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Descript { get; set; }
    }
}
