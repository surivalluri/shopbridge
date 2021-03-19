using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ShopBridge.Api.Interfaces
{
    /// <summary>
    /// SqlDb context to interact with the SQL Server.
    /// </summary>
    public interface ISqlDbContext
    {
        /// <summary>
        /// Executes the sql store procedure.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="sqlParameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DataTable> Execute(string storedProcedureName, IEnumerable<SqlParameter> sqlParameters, CancellationToken cancellationToken);
    }
}
