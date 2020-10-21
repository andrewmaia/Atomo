using System;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Web.Caching;


namespace Atomo.Data
{
    public class SqlDependencyExpiration : ICacheItemExpiration
    {
        SqlDependency sqlDependency;

        public SqlDependencyExpiration(SqlCommand sqlCommand)
        {
            sqlDependency = new SqlDependency(sqlCommand);
        }

        public bool HasExpired()
        {
            return sqlDependency.HasChanges; 
        }
        public void Initialize(CacheItem owningCacheItem)
        {

        }

        public void Notify()
        {
        }
    }
}