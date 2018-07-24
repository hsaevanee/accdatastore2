using System;

namespace ACCDataStore.Helpers.ORM
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionAttribute : Attribute
    {
    }
}
