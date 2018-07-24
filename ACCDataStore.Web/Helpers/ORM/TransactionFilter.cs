﻿using NHibernate;
using System.Web.Mvc;

namespace ACCDataStore.Helpers.ORM
{
    public class TransactionFilter : IActionFilter
    {
        private readonly ISession _session;

        public TransactionFilter(ISession session)
        {
            _session = session;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _session.BeginTransaction();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var transaction = _session.Transaction;
            if (transaction == null)
            {
                return;
            }
            if (!transaction.IsActive)
            {
                return;
            }
            if (filterContext.Exception != null)
            {
                _session.Transaction.Rollback();
            }
            else
            {
                _session.Transaction.Commit();
            }
        }
    }
}