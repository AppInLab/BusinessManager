using System.Collections.Generic;
using BusinessEngine.Models;
using NHibernate;

namespace BusinessEngine.Manager
{
    public class SampleManager
    {
        private static SampleManager _instance;

        public static SampleManager GetInstance()
        {
            if (_instance != null) return _instance;
            _instance = new SampleManager();

            return _instance;
        }

        public List<Categorie> GetData()
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT c FROM Categorie c");
                return (List<Categorie>)req.List<Categorie>();
            }
        }
    }
}