using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Aero.AcceptanceTests.Properties;
using Xunit;

namespace Aero.AcceptanceTests
{
    public class UseDatabaseAttribute : BeforeAfterTestAttribute
    {

        private const string connectionStringName = "AeroContainer";
        public void InstallDatabase()
        {
            var connStr = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            var entityConn = new EntityConnectionStringBuilder(connStr);
            var dbName = new SqlConnectionStringBuilder(entityConn.ProviderConnectionString).InitialCatalog;
            var sysConnection = new SqlConnectionStringBuilder(entityConn.ProviderConnectionString) { InitialCatalog = "Master" };
            ExecuteDbScript(sysConnection.ConnectionString, Resources.DbSchema, new Dictionary<string, string> { { "%dbname%", dbName } });
        }

        public void UninstallDatabase()
        {
            var connStr = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            var entityConn = new EntityConnectionStringBuilder(connStr);
            var dbName = new SqlConnectionStringBuilder(entityConn.ProviderConnectionString).InitialCatalog;
            var sysConnection = new SqlConnectionStringBuilder(entityConn.ProviderConnectionString) { InitialCatalog = "Master" };
            //ExecuteDbScript(sysConnection.ConnectionString, Resources.DbDrop, new Dictionary<string, string> { { "%dbname%", dbName } });
        }

        private void ExecuteDbScript(string connectionString, string script, Dictionary<string, string> scriptParams)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand())
                {
                    var sqlScript = scriptParams.Aggregate(script, (current, scriptParam) => current.Replace(scriptParam.Key, scriptParam.Value));
                    cmd.Connection = conn;
                    foreach (var sql in sqlScript.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }

            }
        }


        public override void Before(System.Reflection.MethodInfo methodUnderTest)
        {
            InstallDatabase();
            base.Before(methodUnderTest);
        }

        public override void After(System.Reflection.MethodInfo methodUnderTest)
        {
            base.After(methodUnderTest);
            UninstallDatabase();
        }
    }
}
