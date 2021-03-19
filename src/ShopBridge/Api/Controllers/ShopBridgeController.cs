using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBridge.Api.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopBridge.Api.Controllers
{
    /// <summary>
    /// Shop bridge controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShopBridgeController : ControllerBase
    {
        private readonly ISqlRepository _sqlRepository;
        private readonly IValidator<Product> _validator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlRepository"></param>
        /// <param name="validator"></param>
        public ShopBridgeController(ISqlRepository sqlRepository,
            IValidator<Product> validator)
        {
            _sqlRepository = sqlRepository ?? throw new System.ArgumentNullException(nameof(sqlRepository));
            _validator = validator ?? throw new System.ArgumentNullException(nameof(validator));
        }

        /// <summary>
        /// Get all the products.
        /// </summary>
        /// <returns></returns>
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _sqlRepository.GetAll(CancellationToken.None);

            if (!products.Any())
                return NotFound();

            return Ok(products);
        }

        /// <summary>
        /// Creates a product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("products")]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var validationResult = _validator.Validate(product);

            if (!validationResult.IsValid)
                return BuildModelValidationResult(validationResult);

            var savedProduct = await _sqlRepository.Get(product.Id, CancellationToken.None);

            if (savedProduct != null)
                return BadRequest(new
                {
                    product.Id,
                    Message = "Product is already created."
                });

            await _sqlRepository.Create(product, CancellationToken.None);

            return Ok(new
            {
                product.Id,
                Message = "Product has been created."
            });
        }

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("products/{productId}")]
        public async Task<IActionResult> Update([FromRoute] string productId, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var validationResult = _validator.Validate(product);

            if (!validationResult.IsValid)
                return BuildModelValidationResult(validationResult);

            var savedProduct = await _sqlRepository.Get(productId, CancellationToken.None);

            if (savedProduct is null)
                return BadRequest(new 
                {
                    Id = productId,
                    Message = "Product is deleted or not found."
                });

            await _sqlRepository.Update(productId, product, CancellationToken.None);

            return Ok(new
            {
                product.Id,
                Message = "Product has been updated."
            });
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("products/{productId}")]
        public async Task<IActionResult> Delete([FromRoute] string productId)
        {
            var product = await _sqlRepository.Get(productId, CancellationToken.None);

            if (product is null)
                return NoContent();

            await _sqlRepository.Delete(productId, CancellationToken.None);

            return Ok(new
            {
                Id = productId,
                Message = "Product has been deleted."
            });
        }

        private ObjectResult BuildModelValidationResult(ValidationResult validationResult) 
        {
            var errors = new List<ActionErrorResponse>();

            foreach (var item in validationResult.Errors)
            {
                var segments = item.PropertyName.Split('.').ToList();

                var parameter = (segments.Count > 1) ? segments.Last() : segments.First();
                var pointer = (segments.Count > 1) ? string.Join("/", segments.Take(segments.Count - 1)) : string.Empty;

                errors.Add(new ActionErrorResponse
                {
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Title = $"INVALID {parameter}",
                    Detail = item.ErrorMessage,
                    Source = new ObjectSource 
                    {
                        Pointer = string.IsNullOrWhiteSpace(pointer) ? "$" : $"$.{pointer}",
                        Parameter = parameter
                    }
                });
            }

            return StatusCode(StatusCodes.Status422UnprocessableEntity, new ActionResponse 
            {
                Errors = errors
            });
        }
    }

    public class ActionResponse
    {
        public IEnumerable<ActionErrorResponse> Errors { get; set; }
    }

    public class ActionErrorResponse
    {
        public int Status { get; set; }

        public string Title { get; set; }

        public string Detail { get; set; }

        public ObjectSource Source { get; set; }
    }

    public class ObjectSource 
    {
        public string Pointer { get; set; }

        public string Parameter { get; set; }
    }
}
