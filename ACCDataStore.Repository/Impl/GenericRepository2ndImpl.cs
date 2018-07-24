using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace ACCDataStore.Repository.Impl
{
    public class GenericRepository2ndImpl : BaseRepositoryImpl, IGenericRepository2nd
    {
        public GenericRepository2ndImpl(ISession session)
            : base(session)
        {
        }
    }
}
