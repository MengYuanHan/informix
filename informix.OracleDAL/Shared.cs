using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using informix.DBUtility;
using System.Data;
using informix.IDAL;

namespace informix.OracleDAL
{
    public class Shared:IShared
    {
        /// <summary>
        /// 获取目录当前位置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetLocationById(string id)
        {
            string SQLLOCATION = "SELECT module FROM sys_module start with moduleid="+id+" connect by prior parentid =  moduleid order by level desc";
            DataTable dt = OracleHelper.query(SQLLOCATION).Tables[0];
            
            string col = string.Empty;
            int colNum = dt.Columns.Count;
            int num = dt.Rows.Count;
            StringBuilder sb = new StringBuilder();            

            for (int m = 0; m < num; m++)
            {
                string str = null;
                sb.Append("<li><a href='#'>");                
                str = dt.Rows[m][0].ToString();
                sb.Append(str);
                sb.Append("</a></li>");
            }
            string json = sb.ToString();
            return json;
        }
    }
}
