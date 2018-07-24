using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Helpers.ORM
{
    public interface IBaseUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}
