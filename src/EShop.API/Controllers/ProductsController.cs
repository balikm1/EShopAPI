using System;
using System.Collections.Generic;
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
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductDAO productDAO;
        private readonly ILogger<ProductsController> logger;

        /// <summary>Initializes new instance of <see cref="ProductDAO"/>.</summary>
        public ProductsController(IProductDAO productDAO, ILogger<ProductsController> logger)
        {
            this.productDAO = productDAO ?? throw new ArgumentNullException(nameof(productDAO));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            logger.LogDebug($"GetAllProducts() processed.");

            var products = await productDAO.GetAllProducts();
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            logger.LogDebug($"GetProductById({id}) processed.");

            var product = await productDAO.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> SaveProduct([FromBody] Product newProduct)
        {
            logger.LogDebug($"SaveProduct({newProduct}) processed.");

            await productDAO.SaveProduct(newProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        // PATCH: api/products/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdateProduct(int id, [FromBody] ProductPatchDto patchDto)
        {
            logger.LogDebug($"PartiallyUpdateProduct(id: {id}, {patchDto}) processed.");

            var product = await productDAO.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(patchDto.Name))
            {
                product.Name = patchDto.Name;
            }

            if (!string.IsNullOrEmpty(patchDto.ImgUri))
            {
                product.ImgUri = patchDto.ImgUri;
            }

            if (patchDto.Price.HasValue)
            {
                product.Price = patchDto.Price.Value;
            }

            if (!string.IsNullOrEmpty(patchDto.Description))
            {
                product.Description = patchDto.Description;
            }

            await productDAO.UpdateProduct(product);
            return NoContent();
        }
    }
}
