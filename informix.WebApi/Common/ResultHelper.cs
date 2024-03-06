using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace informix.WebApi.Common
{
    /// <summary>
    /// 接口执行结果类[返回json类型]
    /// </summary>
    public class ResultHelper
    {
        /// <summary>
        /// 执行代码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 代码含义
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 执行结果
        /// </summary>
        public object data { get; set; }
    }
}