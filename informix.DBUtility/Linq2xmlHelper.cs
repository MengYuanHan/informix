using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace informix.DBUtility {
    /// <summary>
    /// 使用linq操作xml辅助类
    /// </summary>
    public class Linq2xmlHelper {
        private static string path = new ConfigHelper().GetConfigPath("config");
        /// <summary>
        /// 获取配置文件操作对象
        /// </summary>
        /// <returns></returns>
        public XDocument Doc()
        {
            //获取xml文件中的数据
            return XDocument.Load(path);
        }
        /// <summary>
        /// 根据父节点元素Id值获取子节点元素值
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="elementIdValue">元素id值</param>
        /// <param name="childElement">子元素</param>
        /// <returns></returns>
        public string GetChildElValueByElIdValue(string element, string elementIdValue, string childElement)
        {
            XDocument xdoc = new Linq2xmlHelper().Doc();

            XElement item = xdoc.Root.Elements(element).Where(x => x.Attribute("id").Value == elementIdValue).Single();

            return item.Element(childElement).Value;
        }
        /// <summary>
        /// 根据节点名和Id修改某个属性值
        /// </summary>
        /// <param name="itemId">节点Id的值</param>
        /// <param name="node">节点名</param>
        /// <param name="attr">属性名</param>
        /// <param name="value">值</param
        /// <param name="type">修改类型1节点2属性</param>
        public void UpdateItemAttribute(string itemId, string node, string attr, string value, int type)
        {
            XDocument xdoc = new Linq2xmlHelper().Doc();
            //获取项
            IEnumerable<XElement> items = from b in xdoc.Descendants(node)
                                          //where b.Attribute("id").Value == itemId
                                          select b;
            //如果数据存在
            if (items.Count() > 0)
            {
                if (type == 1)
                {
                    foreach (XElement item in items)
                    {
                        //修改节点值
                        //item.SetElementValue(node, value);
                        item.SetValue(value);
                    }
                }
                else
                {
                    foreach (XElement item in items)
                    {
                        item.SetAttributeValue(attr, value);
                    }
                }
                xdoc.Save(path);
            }
        }
        /// <summary>
        /// 获取节点属性值
        /// </summary>
        /// <param name="node">节点名</param>
        /// <param name="attr">属性名</param>
        /// <returns></returns>
        public string GetXmlAttributeValue(string node, string attr)
        {
            string content = string.Empty;
            XDocument xdoc = new Linq2xmlHelper().Doc();
            var query = from c in xdoc.Descendants(node)
                        //where c.Attribute("id").Value == itemId
                        select c;
            string result = "";
            if (query.Count() > 0)
            {
                foreach (XElement item in query)
                {
                    result = item.Attribute(attr).Value;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取节点属性值
        /// </summary>
        /// <param name="node">节点名</param>
        /// <param name="attr">属性名</param>
        /// <returns></returns>
        public string GetXmlAttributeValueById(string node,string itemId, string attr)
        {
            string content = string.Empty;
            XDocument xdoc = new Linq2xmlHelper().Doc();
            var query = from c in xdoc.Descendants(node)
                            where c.Attribute("id").Value == itemId
                        select c;
            string result = "";
            if (query.Count() > 0)
            {
                foreach (XElement item in query)
                {
                    result = item.Attribute(attr).Value;
                }
            }
            return result;
        }
    }
}
