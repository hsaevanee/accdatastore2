using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Repository
{
    public interface IBaseRepository
    {
        IList<T> FindAll<T>() where T : class;
        T FindById<T>(object id);
        T GetById<T>(object id);
        IList<T> Find<T>(string queryString, string[] names, object[] values);
        object FindUniqueResult<T>(string queryString, string[] names, object[] values);
        IList<object[]> FindByNativeSQL(string queryString);
        IList<object> FindSingleColumnByNativeSQL(string queryString);
        void SaveOrUpdate<T>(T entity);
        void Merge<T>(T entity) where T : class;
        void Delete<T>(T entity);
        IQueryOver<T, T> QueryOver<T>() where T : class;
        IQueryable<T> Query<T>() where T : class;
    }
}
