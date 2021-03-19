using Microsoft.Extensions.Options;
using ShopBridge.Api.Configurations;
using ShopBridge.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace ShopBridge.Api.Repositories
{
    public class SqlDbContext : ISqlDbContext
    {
        private readonly SqlConfiguration _sqlConfiguration;

        public SqlDbContext(IOptions<SqlConfiguration> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _sqlConfiguration = options.Value;
        }

        public async Task<DataTable> Execute(string storedProcedureName, IEnumerable<SqlParameter> sqlParameters, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(storedProcedureName))
                throw new ArgumentNullException(nameof(storedProcedureName));

            var directory = Directory.GetCurrentDirectory();

            var connectionString = string.Format(_sqlConfiguration.ConnectionString, $"{directory}\\Datastore\\");

            using var sqlConnection = new SqlConnection(connectionString);

            using var sqlCommand = new SqlCommand(storedProcedureName, sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCommand.Parameters.AddRange(sqlParameters.ToArray());

            await sqlConnection.OpenAsync(cancellationToken);

            using var sqlReader = await sqlCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);

            var dt = new DataTable();

            dt.Load(sqlReader);

            return dt;
        }
    }
}
