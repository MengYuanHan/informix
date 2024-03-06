using System.Web.Http;
using WebActivatorEx;
using informix.WebApi;
using Swashbuckle.Application;
using informix.WebApi.Common;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace informix.WebApi
{
    /// <summary>
    /// 注册配置类
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// swagger注册
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "信息管理系统公共接口");
                        c.IncludeXmlComments(GetXmlCommentsPath());
                        //添加文件上传参数
                        c.OperationFilter<SwaggerFileUploadFilter>();
                    })
                .EnableSwaggerUi(c =>
                    {
                        //将界面的英文转成中文
                        c.InjectJavaScript(thisAssembly, "informix.WebApi.Scripts.swagger_lang.js");
                    });
        }
        /// <summary>
        /// swagger配置xml存放路径
        /// </summary>
        /// <returns></returns>
        public static string GetXmlCommentsPath()
        {
            return string.Format("{0}/bin/informix.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
