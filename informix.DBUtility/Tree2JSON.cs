using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace informix.DBUtility.Tree2JSON
{
    public class Tree2JSON
    {
        /// <summary>
        /// 将一棵树的结构数据转化成JSON
        /// </summary>
        /// <param name="uniqueId">惟一标识</param>
        /// <param name="opt">0:从根节点开始； 1:从子节点开始;2:从where添加开始</param>
        /// <param name="tname">表名</param>
        /// <param name="cname">列名</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="orderFile">排序ID</param>
        /// <returns>返回JSON格式的数组字符串</returns>
        public string GetJSON(string uniqueId, string opt, string tname, string cname, string strWhere, string orderFile)
        {
            //为了支持数据结构没有根节点的情况，这里增加直接取子节点的算法
            DataSet ds = new DataSet();
            string rnt = "";
            if (opt == "0")
            {
                rnt = InitJSON(uniqueId, tname, cname, orderFile);
            }
            else
            {
                if (opt == "2")
                {
                    string sql = "SELECT * FROM " + tname + " WHERE " + strWhere + " ORDER BY " + orderFile + "";
                    ds = OracleHelper.query(sql);
                }
                else
                {
                    ds = GetChildren(uniqueId, tname, orderFile);
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    rnt += InitJSON(ds.Tables[0].Rows[i]["uniqueId"].ToString(), tname, cname, orderFile);
                    if (i + 1 < ds.Tables[0].Rows.Count) rnt += ",";
                }
            }

            //还未创建子节点
            if (rnt == "")
            {
                return "[]";
            }

            if (rnt.Substring(rnt.Length - 1) == ",")
            {
                rnt = rnt.Substring(0, rnt.Length - 1);
            }
            rnt = "[" + rnt + "]";
            return rnt;
        }
        /// <summary>
        /// 判断是否为叶节点
        /// </summary>
        /// <param name="uniqueId">惟一标识</param>
        /// <param name="tname">表名</param>
        /// <returns>true or false</returns>
        public static bool IsLeaf(string uniqueId, string tname)
        {
            //判断是否是叶子节点
            string syntax = "";
            int rnt = 0;
            syntax = "SELECT count(*) FROM " + tname + " WHERE ParentId = " + uniqueId + "";
            rnt = int.Parse(OracleHelper.GetSingle(syntax).ToString());
            if (rnt == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获得叶子节点
        /// </summary>
        /// <param name="uniqueId">惟一标识</param>
        /// <param name="tname">表名</param>
        /// <param name="orderFile">排序ID</param>
        /// <returns>查询影响记录</returns>
        public DataSet GetChildren(string uniqueId, string tname, string orderFile)
        {
            //取子节点记录
            string syntax = "";
            syntax = "SELECT * FROM " + tname + " WHERE ParentId = " + uniqueId + " ORDER BY " + orderFile + "";
            return OracleHelper.query(syntax);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="uniqueId">惟一标识</param>
        /// <param name="tname">表名</param>
        /// <param name="cname">列名</param>
        /// <param name="orderFile">排序ID</param>
        /// <returns>字符串</returns>
        public string InitJSON(string uniqueId, string tname, string cname, string orderFile)
        {
            //遍历节点，生成json格式数据
            DataSet ds = new DataSet();
            DataSet ds_rec = new DataSet();
            string str = "";
            string rnt = "";

            ds_rec = GetObject(uniqueId, tname);
            if (ds_rec.Tables[0].Rows.Count == 0) { return rnt; }

            str = ds_rec.Tables[0].Rows[0][cname].ToString();
            string disNum = ""; //GetDisplayNum(ID);//这样写的原因
            str += disNum;
            if (IsLeaf(uniqueId, tname) == false)
            {
                rnt += "{text:'" + str + "',id:" + uniqueId + ",children:[";
                ds = GetChildren(uniqueId, tname, orderFile);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string json = InitJSON(ds.Tables[0].Rows[i]["uniqueId"].ToString(), tname, cname, orderFile);
                    if (json == "")
                    {
                        continue;
                    }
                    rnt += json;
                    if (i + 1 < ds.Tables[0].Rows.Count) rnt += ",";
                }
                if (rnt.Substring(rnt.Length - 1) == ",")
                {
                    rnt = rnt.Substring(0, rnt.Length - 1);
                }
                rnt += "]}";
            }
            else
            {
                rnt = "{text:'" + str + "',id:" + uniqueId + ",leaf:true}";
            }

            return rnt;
        }
        /// <summary>
        /// 获得对象
        /// </summary>
        /// <param name="uniqueId">惟一标识</param>
        /// <param name="tname">表名</param>
        /// <returns></returns>
        public DataSet GetObject(string uniqueId, string tname)
        {
            //获取了一个实体对象
            //这里写成ds的原因是因为前面的函数使用的也是ds，所以这里不写成实体对象
            string syntax = "";
            syntax = "SELECT * FROM " + tname + " WHERE UniqueId = " + uniqueId + "";
            return OracleHelper.query(syntax);

        }

    }

}



