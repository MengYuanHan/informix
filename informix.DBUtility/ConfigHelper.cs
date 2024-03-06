using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace informix.DBUtility {
    /// <summary>
    /// 配置文件辅助类
    /// </summary>
    public class ConfigHelper {
        /// <summary>
        /// 根据配置文件类型获取配置文件路径
        /// </summary>
        /// <param name="type">配置文件类型</param>
        /// <returns></returns>
        public string GetConfigPath(string type) {
            //
            string path = AppDomain.CurrentDomain.BaseDirectory;
            switch (type)
            {
                //配置文件
                case "config":
                    path = path + "configuration\\informix.config.xml";
                    break;
                case "ErrorLog":
                    path = path + "Files\\ErrorLog\\";
                    break;
                case "OperLog":
                    path = path + "Files\\OperLog\\";
                    break;
                case "zip":
                    path = path + "Files\\UploadZipFile\\";
                    break;
            }
            return path;
        }

    }
}
