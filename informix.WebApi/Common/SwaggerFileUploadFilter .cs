using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace informix.WebApi.Common
{
    /// <summary>
    /// swagger file upload parameter filter
    /// </summary>
    public class SwaggerFileUploadFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var parameters = apiDescription.ActionDescriptor.GetParameters();
            foreach (HttpParameterDescriptor parameterDesc in parameters)
            {
                var fileUploadAttr = parameterDesc.GetCustomAttributes<SwaggerFileUploadAttribute>().FirstOrDefault();
                if (fileUploadAttr != null)
                {
                    operation.consumes.Add("multipart/form-data");

                    operation.parameters.Add(new Parameter
                    {
                        name = parameterDesc.ParameterName + "_file",
                        @in = "form-data",
                        description = "上传文件",
                        required = fileUploadAttr.Required,
                        type = "file"
                    });
                }
            }
        }
    }
}