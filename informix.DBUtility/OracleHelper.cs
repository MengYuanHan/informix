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
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
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
        /// 获得表名
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static string GetTableName(string sql)
        {
            //将语句中的关键字符处理成小写
            string tblName = sql.ToLower().Substring(sql.ToLower().IndexOf("FROM") + 5);
            //如果没有where条件，则截取where前的部分
            if (tblName.IndexOf("WHERE") != -1)
            {
                tblName = tblName.Substring(0, tblName.IndexOf("WHERE"));
            }
            return tblName.Trim();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <returns>查询结果集</returns>
        public static DataSet query(string query)
        {
            //创建连接
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                //创建结果集
                DataSet ds = new DataSet();
                try
                {
                    //打开连接
                    connection.Open();
                    //设置适配器
                    OracleDataAdapter command = new OracleDataAdapter(query, connection);
                    //装填结果集
                    command.Fill(ds, GetTableName(query));
                }
                catch (OracleException ex)
                {
                    //抛出异常
                    throw new Exception(ex.Message);
                }
                //返回结果集
                return ds;
            }
        }
        /// <summary>
        /// 执行SQL语句，返回一条记录的第一列
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
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
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
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
