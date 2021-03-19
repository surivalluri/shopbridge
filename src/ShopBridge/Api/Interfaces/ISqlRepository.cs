using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopBridge.Api.Interfaces
{
    /// <summary>
    /// Sql repository to perform the CRUD operations on the data store
    /// </summary>
    public interface ISqlRepository
    {
        /// <summary>
        /// Create a product.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Create(Product product, CancellationToken cancellationToken);

        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Update(string id, Product product, CancellationToken cancellationToken);

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Delete(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Get all products
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken);

        /// <summary>
        /// Get a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Product> Get(string id, CancellationToken cancellationToken);
    }
}
