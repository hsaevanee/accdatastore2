using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace ACCDataStore.Repository.Impl
{
    public class GenericRepositoryImpl : BaseRepositoryImpl, IGenericRepository
    {
        public GenericRepositoryImpl(ISession session) : base(session)
        {
        }
    }
}
