using System.Web.Http;
using WebActivatorEx;
using informix.WebApi;
using Swashbuckle.Application;
using informix.WebApi.Common;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace informix.WebApi
{
    /// <summary>
    /// ע��������
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// swaggerע��
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "��Ϣ����ϵͳ�����ӿ�");
                        c.IncludeXmlComments(GetXmlCommentsPath());
                        //����ļ��ϴ�����
                        c.OperationFilter<SwaggerFileUploadFilter>();
                    })
                .EnableSwaggerUi(c =>
                    {
                        //�������Ӣ��ת������
                        c.InjectJavaScript(thisAssembly, "informix.WebApi.Scripts.swagger_lang.js");
                    });
        }
        /// <summary>
        /// swagger����xml���·��
        /// </summary>
        /// <returns></returns>
        public static string GetXmlCommentsPath()
        {
            return string.Format("{0}/bin/informix.WebApi.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
