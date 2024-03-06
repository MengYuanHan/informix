using System;

namespace informix.WebApi.Common
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SwaggerFileUploadAttribute : Attribute
    {
        public bool Required { get; private set; }

        public SwaggerFileUploadAttribute(bool Required = true)
        {
            this.Required = Required;
        }
    }
}