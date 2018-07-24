using NHibernate;

namespace ACCDataStore.Helpers.ORM
{
    public interface INHibernateHelper
    {
        ISessionFactory CreateSessionFactory(); //connect with MSAccess database2
        ISessionFactory CreateSessionFactory2nd(); //connect with MySQL
        ISessionFactory CreateSessionFactory3nd(); //connect with MSAccess ScotXed_15
    }
}
