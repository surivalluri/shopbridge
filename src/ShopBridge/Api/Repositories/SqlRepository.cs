using Microsoft.Data.SqlClient;
using ShopBridge.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShopBridge.Api.Repositories
{
    /// <summary>
    /// Sql Repository
    /// </summary>
    public class SqlRepository : ISqlRepository
    {
        private readonly ISqlDbContext _sqlDbContext;

        public SqlRepository(ISqlDbContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext ?? throw new ArgumentNullException(nameof(sqlDbContext));
        }

        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Create(Product product, CancellationToken cancellationToken)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            var parameters = new List<SqlParameter>
            { 
                new SqlParameter
                {
                    ParameterName = "@tbl_Product",
                    Direction = System.Data.ParameterDirection.Input,
                    Value = product.AsSqlDataRecord(),
                    SqlDbType = System.Data.SqlDbType.Structured
                }
            };

            await _sqlDbContext.Execute("CreateProduct", parameters, cancellationToken);
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "@Id",
                    Direction = System.Data.ParameterDirection.Input,
                    Value = id,
                    SqlDbType = System.Data.SqlDbType.NVarChar
                }
            };

            await _sqlDbContext.Execute("DeleteProduct", parameters, cancellationToken);
        }

        /// <summary>
        /// Get a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Product> Get(string id, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "@Id",
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.NVarChar,
                    Value = id
                }
            };

            var records = await _sqlDbContext.Execute("GetProduct", parameters, cancellationToken);

            Product product = null;

            if (records.Rows.Count >= 1)
                foreach (DataRow item in records.Rows)
                {
                    product = JsonSerializer.Deserialize<Product>(item["Product"].ToString());
                }

            return product;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken)
        {
            var records = await _sqlDbContext.Execute("GetProducts", Enumerable.Empty<SqlParameter>(), cancellationToken);

            var lst = new List<Product>();

            if(records.Rows.Count >= 1)
                foreach (DataRow item in records.Rows)
                {
                    lst.Add(JsonSerializer.Deserialize<Product>(item["Product"].ToString()));
                }

            return lst;
        }

        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Update(string id, Product product, CancellationToken cancellationToken)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            var parameters = new List<SqlParameter>
            {
                new SqlParameter
                {
                    ParameterName = "@tbl_Product",
                    Direction = ParameterDirection.Input,
                    Value = product.AsSqlDataRecord(),
                    SqlDbType = SqlDbType.Structured
                },
                new SqlParameter
                {
                    ParameterName = "@Id",
                    Direction = ParameterDirection.Input,
                    Value = id,
                    SqlDbType = SqlDbType.NVarChar
                }
            };

            await _sqlDbContext.Execute("UpdateProduct", parameters, cancellationToken);
        }
    }
}
