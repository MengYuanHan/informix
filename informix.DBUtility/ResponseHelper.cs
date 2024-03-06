using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace informix.DBUtility {
    /// <summary>
    /// 消息提示类[使用排序的字典类(SortedDictionary)]
    /// </summary>
    public class ResponseHelper {
        //申明一个自动排序的键值对的字典
        public SortedDictionary<string, string> m_values = new SortedDictionary<string, string>();

        /// <summary>
        /// 获取里面的键值对字典
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, string> GetKeyValuePair()
        {
            return m_values;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (m_values.ContainsKey(key))
            {
                return m_values[key];
            }
            return "";
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetValue(string key, string value)
        {
            if (m_values.ContainsKey(key))
            {
                m_values[key] = value;
            }
            else
            {
                m_values.Add(key, value);
            }
        }



        /// <summary>
        /// 获取默认的执行成功时的json
        /// </summary>
        /// <param name="trueMessage">执行成功时的提示文本</param>
        /// <returns></returns>
        public string GetSuccessJson(string trueMessage)
        {
            SetValue("retKey", "Y");
            SetValue("retValue", trueMessage);

            return JsonConvert.SerializeObject(m_values);
        }

        /// <summary>
        /// 获取默认的执行失败时的json
        /// </summary>
        /// <param name="falseMessage">执行失败时的提示文本</param>
        /// <returns></returns>
        public string GetFailJson(string falseMessage)
        {          
            SetValue("retKey", "N");
            SetValue("retValue", falseMessage);

            return JsonConvert.SerializeObject(m_values);
        }
        /// <summary>
        /// 将对象转成json字符串[序列化]
        /// </summary>
        /// <returns></returns>
        public string ObjectToJson()
        {
            return JsonConvert.SerializeObject(m_values);
        }
        /// <summary>
        /// 将json字符串转成对象[反序列化]
        /// </summary>
        /// <param name="result">JSON字符串</param>
        /// <returns>有序的字典JSON对象</returns>

        public SortedDictionary<string, string> jsonToObject(string result)
        {
            result = result.Replace("[", "").Replace("]", "");
            SortedDictionary<string, string> obj = JsonConvert.DeserializeObject<SortedDictionary<string, string>>(result);
            return obj;
        }
    }
}
