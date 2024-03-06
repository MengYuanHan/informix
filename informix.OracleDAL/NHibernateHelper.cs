using NHibernate;
using NHibernate.Cfg;

namespace informix.OracleDAL
{
    public class NHibernateHelper
    {
        /// <summary>
        /// 创建会话工厂
        /// </summary>
        private ISessionFactory _sessionFactory;
        /// <summary>
        /// 将创建的会话工厂传给配置的会话工厂
        /// </summary>
        public NHibernateHelper()
        {
            _sessionFactory = GetSessionFactory();
        }
        /// <summary>
        /// 配置中创建会话工厂
        /// </summary>
        /// <returns></returns>
        private ISessionFactory GetSessionFactory()
        {
            return (new Configuration()).Configure().BuildSessionFactory();
        }
        /// <summary>
        /// 打开会话
        /// </summary>
        /// <returns></returns>
        public ISession GetSession()
        {
            return _sessionFactory.OpenSession();
        }

    }
}
