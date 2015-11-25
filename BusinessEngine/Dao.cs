using System;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;

namespace BusinessEngine
{
    public class Dao
    {
        private static ISessionFactory _sessionFactory; 

        private readonly string _configFile;

        public Dao(string configFile)
        {
            this._configFile = configFile;
        }

        private void OpenSession(string configFile)
        {
            {
                _sessionFactory = new Configuration().Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile)).BuildSessionFactory();
            }
        }

        public ISession GetSession()
        {
            if (_sessionFactory == null)
                this.OpenSession(this._configFile);

            return _sessionFactory.OpenSession();
        }

        private static void OpenSession()
        {
            var configuration = new Configuration();
            _sessionFactory = new Configuration().Configure().BuildSessionFactory();
            configuration.AddAssembly(Assembly.GetCallingAssembly());
            _sessionFactory = configuration.BuildSessionFactory();
        }

        public static ISession GetCurrentSession()
        {
            if (_sessionFactory == null)
                Dao.OpenSession();

            return _sessionFactory.OpenSession();
        }

        public static void CloseSessionFactory()
        {
            if (_sessionFactory != null)
                _sessionFactory.Close();
        }
    }
}
