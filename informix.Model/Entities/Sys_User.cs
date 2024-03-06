using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.Model.Entities {
    public class Sys_User {
        /// <summary>
        /// 用户编号
        /// </summary>
        public virtual int UserId { get; set; }
        /// <summary>
        /// 登陆名
        /// </summary>
        public virtual string UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public virtual string Password { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public virtual int DeptId { get; set; }
        /// <summary>
        /// 真实姓名名
        /// </summary>
        public virtual string RealName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public virtual string Sex { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public virtual string Phone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public virtual string Fax { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public virtual string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public virtual string City { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public virtual string QQ { get; set; }
        /// <summary>
        /// 诨名
        /// </summary>
        public virtual string NickName { get; set; }
        /// <summary>
        /// 创建用户编号
        /// </summary>
        public virtual int CreateUserId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual string State { get; set; }

    }
}
