using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
//using FluentNHibernate;
//using FluentNHibernate.Cfg.Db;

namespace informix.OracleDAL
{
    /// <summary>
    /// Nhibernate辅助类
    /// </summary>
    public class FluentNHibernateHelper
    {
        //private static ISessionFactory _sessionFactory;
        //private static ISession _session;
        //private static object _objLock = new object();
        private FluentNHibernateHelper()
        {

        }
        /// <summary>
        /// 创建ISessionFactory
        /// </summary>
        /// <returns></returns>
        //public static ISessionFactory GetSessionFactory()
        //{
        //    string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.1.1.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=glxt)));User Id=informix;Password=gmgl;";
        //    if (_sessionFactory == null)
        //    {
        //        lock (_objLock)
        //        {
                    //if (_sessionFactory == null)
                    //{
                    //    //配置ISessionFactory
                    //    _sessionFactory = FluentNHibernate.Cfg.Fluently.Configure()
                    //   //数据库配置
                    //  .Database(
                    //  OracleClientConfiguration.Oracle10
                    //  .ConnectionString(connectionString)
                    //  .Provider<NHibernate.Connection.DriverConnectionProvider>()
                    //  .Driver<NHibernate.Driver.OracleClientDriver>()
                    //)
                    ////映射程序集
                    //.Mappings(m => m.FluentMappings
                    //    .AddFromAssembly(System.Reflection.Assembly.Load("informix.Model"))
                    //    .ExportTo("c:\\"))
                    //.BuildSessionFactory();

                    //}
        //        }
        //    }
        //    return _sessionFactory;

        //}
        /// <summary>
        /// 重置Session
        /// </summary>
        /// <returns></returns>
        //public static ISession ResetSession()
        //{
        //    if (_session.IsOpen)
        //        _session.Close();
        //    _session = _sessionFactory.OpenSession();
        //    return _session;
        //}
        /// <summary>
        /// 打开ISession
        /// </summary>
        /// <returns></returns>
        //public static ISession GetSession()
        //{
        //    GetSessionFactory();
        //    if (_session == null)
        //    {
        //        lock (_objLock)
        //        {
        //            if (_session == null)
        //            {
        //                _session = _sessionFactory.OpenSession();
        //            }
        //        }
        //    }
        //    return _session;
        //}

    }
}
