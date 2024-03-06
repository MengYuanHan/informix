using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace informix.WebApi
{
    /// <summary>
    /// WebApi静态配置类
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 路由注册
        /// </summary>
        /// <param name="config">配置</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "SwaggerApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
