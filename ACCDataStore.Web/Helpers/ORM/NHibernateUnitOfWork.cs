using NHibernate;
using System;

namespace ACCDataStore.Helpers.ORM
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;
        private ITransaction _transaction;

        public NHibernateUnitOfWork(ISession session)
        {
            _session = session;
            _transaction = session.BeginTransaction();
        }

        public void Dispose()
        {
            if (_transaction == null)
                return;

            if (_transaction.IsActive && !_transaction.WasCommitted)
                _transaction.Rollback();

            _transaction.Dispose();
            _transaction = null;
        }

        public void SaveChanges()
        {
            _transaction.Commit();
        }

        public void Commit()
        {
            if (!_transaction.IsActive)
            {
                throw new InvalidOperationException("No active transation");
            }
            _transaction.Commit();
        }

        public void Rollback()
        {
            if (_transaction.IsActive)
            {
                _transaction.Rollback();
            }
        }
    }
}
