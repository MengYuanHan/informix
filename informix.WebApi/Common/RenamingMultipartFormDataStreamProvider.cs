using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace informix.WebApi.Common
{
    /// <summary>
    /// 创建一个 Provider 用于重命名接收到的文件
    /// </summary>
    public class RenamingMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public RenamingMultipartFormDataStreamProvider(string path)
           : base(path)
        { }
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            var sb = new StringBuilder((headers.ContentDisposition.FileName ?? DateTime.Now.Ticks.ToString()).Replace("\"", "").Trim().Replace(" ", "_"));
            Array.ForEach(Path.GetInvalidFileNameChars(), invalidChar => sb.Replace(invalidChar, '-'));
            return sb.ToString();
        }
    }
}