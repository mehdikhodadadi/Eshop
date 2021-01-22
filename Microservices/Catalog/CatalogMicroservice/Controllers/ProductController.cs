using ProductModel;
using ProductRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ProductMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            _productRepository.DeleteProduct(id);
            return new OkResult();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(Guid id)
        {
            var product = _productRepository.GetProduct(id);
            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Product product)
        {
            _productRepository.InsertProduct(product);
            return Ok(product);
        }

        [HttpPut]
        public ActionResult Put([FromBody] Product product)
        {
            if (product != null)
            {
                _productRepository.UpdateProduct(product);
                return new OkResult();
            }

            return new NoContentResult();
        }
    }
}