using Microsoft.Data.SqlClient.Server;
using System.Collections.Generic;
using System.Text.Json;

namespace ShopBridge.Api.Repositories
{
    public static class SqlDataRecords
    {
        public static IEnumerable<SqlDataRecord> AsSqlDataRecord(this Product product) 
        {
            var record = new SqlDataRecord(new SqlMetaData[]
            {
                new SqlMetaData("Id", System.Data.SqlDbType.NVarChar, 64),
                new SqlMetaData("Name", System.Data.SqlDbType.NVarChar, 64),
                new SqlMetaData("Description", System.Data.SqlDbType.NVarChar, 256),
                new SqlMetaData("Price", System.Data.SqlDbType.Decimal),
                new SqlMetaData("Product", System.Data.SqlDbType.NVarChar, SqlMetaData.Max),
                new SqlMetaData("RecordStatus", System.Data.SqlDbType.TinyInt)
            });

            record.SetString(0, product.Id);
            record.SetString(1, product.Name);
            record.SetString(2, product.Description);
            record.SetDecimal(3, product.Price);
            record.SetString(4, JsonSerializer.Serialize(product));
            record.SetByte(5, 1);

            return new List<SqlDataRecord> { record };
        }
    }

    public enum RecordStatus 
    {
        Deleted = 0,
        Active = 1
    }
}
