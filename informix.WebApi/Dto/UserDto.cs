using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace informix.WebApi.Dto
{
    /// <summary>
    /// 用户Dto
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 登陆名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
}