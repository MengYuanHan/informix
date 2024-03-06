using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace informix.Web.Dto
{
    public class UserDto
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public  int userId { get; set; }
        /// <summary>
        /// 登陆名
        /// </summary>
        public  string userName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public  string password { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public  int deptId { get; set; }
        /// <summary>
        /// 真实姓名名
        /// </summary>
        public  string realName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public  string title { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public  string sex { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public  string phone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public  string fax { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public  string province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public  string city { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public  string address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public  string email { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public  string QQ { get; set; }
        /// <summary>
        /// 诨名
        /// </summary>
        public  string nickName { get; set; }
        /// <summary>
        /// 创建用户编号
        /// </summary>
        public  int createUserId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public  DateTime createDate { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public  string state { get; set; }

        public string roleId { get; set; }
        
    }
}