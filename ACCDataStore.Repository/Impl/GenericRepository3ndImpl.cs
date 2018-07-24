using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace ACCDataStore.Repository.Impl
{
    public class GenericRepository3ndImpl : BaseRepositoryImpl, IGenericRepository3nd
    {
        public GenericRepository3ndImpl(ISession session)
            : base(session)
        {
        }
    }
}
