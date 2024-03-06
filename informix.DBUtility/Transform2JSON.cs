using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Data;

namespace informix.DBUtility {
    public class Transform2JSON {
        public Transform2JSON()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 将DataTable中的数据转换成json格式的数据
        /// <summary>
        /// 将DataTable中的数据转换成json格式的数据
        /// </summary>
        /// <param name="dt">包含数据的DataTable</param>
        /// <returns>返回{total:xx,rows:[{},{}]}形式的数据</returns>
        public static string ToJSONString(DataTable dt, int count)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("{");    //</br>
            strBuilder.Append("\"total\":" + count.ToString() + ",");
            strBuilder.Append("\"rows\":[");
            int num = dt.Rows.Count;
            if (num == 0)
            {
                return strBuilder.Append("]}").ToString();
            }

            string col = string.Empty;
            int colNum = dt.Columns.Count;

            for (int i = 0; i < colNum; i++)
            {
                col += "\"" + dt.Columns[i].ColumnName + "\":{" + i + "},";
            }
            col = col.Substring(0, col.LastIndexOf(","));

            object[] obj = new object[colNum];
            for (int m = 0; m < num; m++)
            {
                strBuilder.Append("{");
                for (int n = 0; n < colNum; n++)
                {
                    string tp = dt.Rows[m][n].GetType().ToString();
                    obj[n] = null;
                    if (tp == "System.Decimal")
                    {
                        obj[n] = dt.Rows[m][n].ToString();
                    }
                    else if (tp == "System.DateTime")
                    {
                        obj[n] = "\"" + Convert.ToDateTime(dt.Rows[m][n]).ToString("yyyy-MM-dd HH:mm:ss") + "\"";

                    }
                    else
                    {
                        obj[n] = "\"" + dt.Rows[m][n].ToString().Replace("'", "\\'").Replace("\"", "\\\"").Replace(":", "_58_").Replace("：", "_41914_") + "\"";
                    }
                }

                string str = string.Format(col, obj);
                strBuilder.Append(str);
                strBuilder.Append("},");
            }

            strBuilder.Append("]");
            strBuilder.Append("}");

            return strBuilder.ToString().Remove(strBuilder.ToString().LastIndexOf(","), 1);
        }

        #endregion


        #region 将DataTable转换成json格式(OperationSchedule手术日程专用)
        /// <summary>
        /// 将DataTable中的数据转换成json格式的数据(OperationSchedule手术日程专用)
        /// </summary>
        /// <param name="dt">包含数据的DataTable</param>
        /// <returns>返回{total:xx,rows:[{},{}]}形式的数据</returns>
        public static string OSToJSONString(DataTable dt)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("{");    //</br>
            int num = dt.Rows.Count;
            strBuilder.Append("total:" + num.ToString());
            strBuilder.Append(",rows:[");

            if (num == 0)
            {
                return strBuilder.Append("]}").ToString();
            }

            string col = string.Empty;
            int colNum = dt.Columns.Count;

            for (int i = 0; i < colNum; i++)
            {
                col += dt.Columns[i].ColumnName + ":{" + i + "},";
            }
            col = col.Substring(0, col.LastIndexOf(","));

            for (int m = 0; m < num; m++)
            {
                object[] obj = new object[colNum];
                strBuilder.Append("{");
                for (int n = 0; n < colNum; n++)
                {
                    string tp = dt.Rows[m][n].GetType().ToString();
                    obj[n] = null;
                    if (tp == "System.Decimal")
                    {
                        obj[n] = dt.Rows[m][n].ToString();
                    }
                    if (tp == "System.DateTime")
                    {
                        obj[n] = "'" + Convert.ToDateTime(dt.Rows[m][n]).ToShortDateString() + "'";
                    }
                    obj[n] = "'" + dt.Rows[m][n].ToString() + "'";
                }
                string str = string.Format(col, obj);
                strBuilder.Append(str);
                strBuilder.Append("},");
            }

            strBuilder.Append("]");
            strBuilder.Append("}");

            return strBuilder.ToString().Remove(strBuilder.ToString().LastIndexOf(","), 1);
        }
        #endregion

        #region 将List<>转换为Json
        public static string List2JSON(List<object> objlist, string classname)
        {
            string result = "{";
            if (classname.Equals(string.Empty))//如果没有给定类的名称，那么自做聪明地安一个
            {
                object o = objlist[0];
                classname = o.GetType().ToString();
            }
            result += "\"" + classname + "\":[";
            bool firstline = true;//处理第一行前面不加","号
            foreach (object oo in objlist)
            {
                if (!firstline)
                {
                    result = result + "," + OneObjectToJSON(oo);
                }
                else
                {
                    result = result + OneObjectToJSON(oo) + "";
                    firstline = false;
                }
            }
            return result + "]}";
        }

        private static string OneObjectToJSON(object o)
        {
            string result = "{";
            List<string> ls_propertys = new List<string>();
            ls_propertys = GetObjectProperty(o);
            foreach (string str_property in ls_propertys)
            {
                if (result.Equals("{"))
                {
                    result = result + str_property;
                }
                else
                {
                    result = result + "," + str_property + "";
                }
            }
            return result + "}";
        }

        private static List<string> GetObjectProperty(object o)
        {
            List<string> propertyslist = new List<string>();
            PropertyInfo[] propertys = o.GetType().GetProperties();
            foreach (PropertyInfo p in propertys)
            {
                propertyslist.Add("\"" + p.Name.ToString() + "\":\"" + p.GetValue(o, null) + "\"");
            }
            return propertyslist;
        }

        #endregion

        #region 转换JSON字符串到List
        public static List<Hashtable> JSONConvert(string json)
        {
            json.Split('{')[1].Replace("}", "").Split(',')[0].Split(':')[1].Replace("\"", "");
            //根据'{'左大括号得到字符数组
            string[] jsonRow = json.Split('{');

            List<string[]> lst = new List<string[]>();
            foreach (string str in jsonRow)
            {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                    lst.Add(str.Replace("}", "").Split(','));
                }
            }

            List<Hashtable> htList = new List<Hashtable>();
            //string[] temp = null;
            foreach (string[] str in lst)
            {
                Hashtable ht = new Hashtable();
                for (int i = 0; i < str.Length; i++)
                {
                    if (!string.IsNullOrEmpty(str[i].Trim()))
                    {
                        //temp = str[i].Split(':');
                        string temp0 = str[i].Substring(0, str[i].IndexOf(":"));
                        string temp1 = str[i].Substring(str[i].IndexOf(":") + 1);

                        ht.Add(temp0.Replace("\"", "").Trim(), temp1.Replace("\"", "").Replace("_58_", ":").Replace("_41914_", "：").Trim());
                    }
                }
                htList.Add(ht);
            }
            return htList;
        }


        #endregion


        #region ... 生成Id,日期+1000以内的随机数
        public static string CreateId()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString().Replace(":", "").Replace("-", "").Replace(" ", ""));
            sb.Append(Guid.NewGuid().ToString().Substring(0, 4).ToUpper());
            return sb.ToString();
        }
        #endregion


        #region 利用反射赋值个对象属性
        /// <summary>
        /// 利用反射赋值个对象属性
        /// </summary>
        /// <param name="list">转后后的list</param>
        /// <param name="obj">对象类型</param>
        /// <param name="objlist">原数据</param>
        public static List<Object> Transform(Object obj, List<Hashtable> objlist)
        {
            List<Object> list = new List<object>();
            //获取对象类型并序列化
            PropertyInfo[] myPropertyInfo = obj.GetType().GetProperties();
            //循环读取每个Hashtable
            for (int i = 0; i < objlist.Count; i++)
            {
                Hashtable fm = objlist[i];

                int length = myPropertyInfo.Length;

                Object ob = obj;
                //循环序列化属性
                for (int y = 0; y < length; y++)
                {
                    //判断是否存在在Hashtable
                    if (fm.Contains((myPropertyInfo[y].Name).ToString()))
                    {
                        //判断是否为空
                        if (fm[myPropertyInfo[y].Name].ToString() != "" && fm[myPropertyInfo[y].Name.ToString()].ToString() != "undefined")
                        {
                            //存储(根据属性类型进行转换数据)
                            myPropertyInfo[y].SetValue(ob, Convert.ChangeType(fm[myPropertyInfo[y].Name], myPropertyInfo[y].PropertyType), null);

                        }
                    }
                }
                //存储list 
                list.Add(ob);
            }
            return list;
        }
        #endregion


        #region  自动补全
        /// <summary>
        /// 自动补全
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string AutoComplete(string data)
        {
            int count = 8 - data.Length;
            string L = "";
            for (int i = 0; i < count; i++)
            {
                L += "0";
            }
            return L + data;
        }
        #endregion

    }
}
