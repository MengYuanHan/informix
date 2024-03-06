using System.Configuration;
using System.Reflection;

namespace informix.DALFactory
{
    /// <summary>
    /// 此类实现数据访问层的工厂模式
    /// </summary>
    public sealed class DataAccess {
        private static readonly string _type = ConfigurationManager.AppSettings["DBType"];
        private static readonly string _sqlserverPath = ConfigurationManager.AppSettings["SQLServerDAL"];
        private static readonly string _oraclePath = ConfigurationManager.AppSettings["OracleDAL"];
        private static readonly string _mysqlPath = ConfigurationManager.AppSettings["MySqlDAL"];
        /// <summary>
        /// 根据数据库类型获取反射层
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static string path(string dbType){
            string path = "";
            switch (dbType)
            {
                case "Oracle":
                    path = _oraclePath;
                    break;
                case "Ms":
                    path = _sqlserverPath;
                    break;
                case "MySql":
                    path = _mysqlPath;
                    break;
            }
            return path;
        }
        public DataAccess() { }
        public static IDAL.IUser CreateUser() {
            string className = path(_type) + ".User";
            return (IDAL.IUser)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IUserRole CreateUserRole() {
            string className = path(_type) + ".UserRole";
            return (IDAL.IUserRole)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IShared CreateShared() {
            string className = path(_type) + ".Shared";
            return (IDAL.IShared)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IRole CreateRole() {
            string className = path(_type) + ".Role";
            return (IDAL.IRole)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IOnline CreateOnline() {
            string className = path(_type) + ".Online";
            return (IDAL.IOnline)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IModule CreateModule() {
            string className= path(_type) + ".Module";
            return (IDAL.IModule)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IDepartment CreateDepartment() {
            string className = path(_type) + ".Department";
            return (IDAL.IDepartment)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IAuthRole CreateAuthRole() {
            string className = path(_type) + ".AuthRole";
            return (IDAL.IAuthRole)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IAuthority CreateAuthority() {
            string className = path(_type) + ".Authority";
            return (IDAL.IAuthority)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IAuthModule CreateAuthModule() {
            string className = path(_type) + ".AuthModule";
            return (IDAL.IAuthModule)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IApplication CreateApplication() {
            string className = path(_type) + ".Application";
            return (IDAL.IApplication)Assembly.Load(path(_type)).CreateInstance(className);
        }
        public static IDAL.IOperateLog CreateOperateLog() {
            string className = path(_type) + ".OperateLog";
            return (IDAL.IOperateLog)Assembly.Load(path(_type)).CreateInstance(className);
        }
    }
}
