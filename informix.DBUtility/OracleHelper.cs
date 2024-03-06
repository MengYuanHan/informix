using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace informix.DBUtility
{
    public abstract class OracleHelper
    {
        public static readonly string connectionString = ConfigurationManager.AppSettings["Oracle"];

        /// <summary>
        /// ִ��SQL��䣬����Ӱ��ļ�¼��
        /// </summary>
        /// <param name="SQLString">SQL���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
        public static int ExecuteSQL(string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows; 
                    }
                    catch (OracleException e)
                    {
                        connection.Close();
                        throw e; 
                    }
                }
            }
        }

        /// <summary>
        /// ��ñ���
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <returns></returns>
        public static string GetTableName(string sql)
        {
            //������еĹؼ��ַ������Сд
            string tblName = sql.ToLower().Substring(sql.ToLower().IndexOf("FROM") + 5);
            //���û��where���������ȡwhereǰ�Ĳ���
            if (tblName.IndexOf("WHERE") != -1)
            {
                tblName = tblName.Substring(0, tblName.IndexOf("WHERE"));
            }
            return tblName.Trim();
        }
        /// <summary>
        /// ��ѯ
        /// </summary>
        /// <param name="query">sql���</param>
        /// <returns>��ѯ�����</returns>
        public static DataSet query(string query)
        {
            //��������
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                //���������
                DataSet ds = new DataSet();
                try
                {
                    //������
                    connection.Open();
                    //����������
                    OracleDataAdapter command = new OracleDataAdapter(query, connection);
                    //װ������
                    command.Fill(ds, GetTableName(query));
                }
                catch (OracleException ex)
                {
                    //�׳��쳣
                    throw new Exception(ex.Message);
                }
                //���ؽ����
                return ds;
            }
        }
        /// <summary>
        /// ִ��SQL��䣬����һ����¼�ĵ�һ��
        /// </summary>
        /// <param name="SQLString">SQL���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
        public static string GetScalar(string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        string colValue = string.IsNullOrEmpty((cmd.ExecuteScalar() == null ? "" : cmd.ExecuteScalar()).ToString()) ? "" : cmd.ExecuteScalar().ToString();
                        if (string.IsNullOrEmpty(colValue))
                        {
                            return "0";
                        }
                        return colValue;
                    }
                    catch (OracleException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
        /// <summary>
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
        /// </summary>
        /// <param name="SQLString">�����ѯ������</param>
        /// <returns>��ѯ�����object��</returns>
        public static object GetSingle(string SQLString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (OracleException ex)
                    {
                        connection.Close();
                        throw ex;
                    }
                }
            }
        }


    }
}
