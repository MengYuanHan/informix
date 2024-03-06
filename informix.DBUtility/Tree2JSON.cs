using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace informix.DBUtility.Tree2JSON
{
    public class Tree2JSON
    {
        /// <summary>
        /// ��һ�����Ľṹ����ת����JSON
        /// </summary>
        /// <param name="uniqueId">Ωһ��ʶ</param>
        /// <param name="opt">0:�Ӹ��ڵ㿪ʼ�� 1:���ӽڵ㿪ʼ;2:��where��ӿ�ʼ</param>
        /// <param name="tname">����</param>
        /// <param name="cname">����</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="orderFile">����ID</param>
        /// <returns>����JSON��ʽ�������ַ���</returns>
        public string GetJSON(string uniqueId, string opt, string tname, string cname, string strWhere, string orderFile)
        {
            //Ϊ��֧�����ݽṹû�и��ڵ���������������ֱ��ȡ�ӽڵ���㷨
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

            //��δ�����ӽڵ�
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
        /// �ж��Ƿ�ΪҶ�ڵ�
        /// </summary>
        /// <param name="uniqueId">Ωһ��ʶ</param>
        /// <param name="tname">����</param>
        /// <returns>true or false</returns>
        public static bool IsLeaf(string uniqueId, string tname)
        {
            //�ж��Ƿ���Ҷ�ӽڵ�
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
        /// ���Ҷ�ӽڵ�
        /// </summary>
        /// <param name="uniqueId">Ωһ��ʶ</param>
        /// <param name="tname">����</param>
        /// <param name="orderFile">����ID</param>
        /// <returns>��ѯӰ���¼</returns>
        public DataSet GetChildren(string uniqueId, string tname, string orderFile)
        {
            //ȡ�ӽڵ��¼
            string syntax = "";
            syntax = "SELECT * FROM " + tname + " WHERE ParentId = " + uniqueId + " ORDER BY " + orderFile + "";
            return OracleHelper.query(syntax);
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="uniqueId">Ωһ��ʶ</param>
        /// <param name="tname">����</param>
        /// <param name="cname">����</param>
        /// <param name="orderFile">����ID</param>
        /// <returns>�ַ���</returns>
        public string InitJSON(string uniqueId, string tname, string cname, string orderFile)
        {
            //�����ڵ㣬����json��ʽ����
            DataSet ds = new DataSet();
            DataSet ds_rec = new DataSet();
            string str = "";
            string rnt = "";

            ds_rec = GetObject(uniqueId, tname);
            if (ds_rec.Tables[0].Rows.Count == 0) { return rnt; }

            str = ds_rec.Tables[0].Rows[0][cname].ToString();
            string disNum = ""; //GetDisplayNum(ID);//����д��ԭ��
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
        /// ��ö���
        /// </summary>
        /// <param name="uniqueId">Ωһ��ʶ</param>
        /// <param name="tname">����</param>
        /// <returns></returns>
        public DataSet GetObject(string uniqueId, string tname)
        {
            //��ȡ��һ��ʵ�����
            //����д��ds��ԭ������Ϊǰ��ĺ���ʹ�õ�Ҳ��ds���������ﲻд��ʵ�����
            string syntax = "";
            syntax = "SELECT * FROM " + tname + " WHERE UniqueId = " + uniqueId + "";
            return OracleHelper.query(syntax);

        }

    }

}



