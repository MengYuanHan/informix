using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace informix.Web.Model
{
    public class User
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public  int UserId { get; set; }
        /// <summary>
        /// 登陆名
        /// </summary>
        public  string UserName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public  string Password { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public  int DeptId { get; set; }
        /// <summary>
        /// 真实姓名名
        /// </summary>
        public  string RealName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public  string Title { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public  string Sex { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public  string Phone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public  string Fax { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public  string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public  string City { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public  string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public  string Email { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public  string QQ { get; set; }
        /// <summary>
        /// 诨名
        /// </summary>
        public  string NickName { get; set; }
        /// <summary>
        /// 创建用户编号
        /// </summary>
        public  int CreateUserId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public  DateTime CreateDate { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public  string State { get; set; }
        //角色Id

        public int RoleId { get; set; }
    }
}