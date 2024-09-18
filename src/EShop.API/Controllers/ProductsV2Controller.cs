using System;
using System.Threading.Tasks;
using Asp.Versioning;
using EShop.Core.Interfaces;
using EShop.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EShop.API.Controllers
{
    [Route("products")]
    [ApiController]
    [ApiVersion("2.0")]
    public class ProductsV2Controller : ControllerBase
    {
        private const int DefaultPageSize = 10;

        private readonly IProductDAO productDAO;
        private readonly ILogger<ProductsV2Controller> logger;

        public ProductsV2Controller(IProductDAO productDAO, ILogger<ProductsV2Controller> logger)
        {
            this.productDAO = productDAO ?? throw new ArgumentNullException(nameof(productDAO));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/v2/products
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<Product>>> GetAllProducts([FromQuery] int page = 1, [FromQuery] int pageSize = DefaultPageSize)
        {
            logger.LogDebug($"GetAllProducts(page: {page}, pageSize: {pageSize}) processed.");

            var products = await productDAO.GetAllProducts(page, pageSize);
            var totalProducts = await productDAO.GetAllProductsCount();

            var result = new PaginatedResult<Product>(products, totalProducts, pageSize, page);
            return Ok(result);
        }
    }
}
