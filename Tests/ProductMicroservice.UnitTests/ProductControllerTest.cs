using ProductMicroservice.Controllers;
using ProductModel;
using ProductRepository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ProductMicroservice.UnitTests
{
    public class ProductControllerTest
    {
        private readonly ProductController _controller;

        private readonly List<Product> _items = new List<Product>
        {
            new Product { Id = new Guid("ce2dbb82-6689-487b-9691-0a05ebabce4a"), Name = "Samsung Galaxy S10", Description = "Samsung Galaxy S10 mobile phone", Price= 1050 },
            new Product { Id = new Guid("13b87ba8-f542-441c-bc2c-db32fb01ec3f"), Name = "Samsung Galaxy S9", Description = "Samsung Galaxy S9 mobile phone", Price= 700 }
        };

        public ProductControllerTest()
        {
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.GetProducts()).Returns(_items);
            mockRepo.Setup(repo => repo.GetProduct(It.IsAny<Guid>())).Returns<Guid>(id => _items.FirstOrDefault(i => i.Id == id));
            mockRepo.Setup(repo => repo.InsertProduct(It.IsAny<Product>())).Callback<Product>(i => _items.Add(i));

            mockRepo.Setup(repo => repo.UpdateProduct(It.IsAny<Product>())).Callback<Product>(i =>
            {
                var item = _items.FirstOrDefault(i => i.Id == i.Id);

                if (item != null)
                {
                    item.Name = i.Name;
                    item.Description = i.Description;
                    item.Price = i.Price;
                }
            });

            mockRepo.Setup(repo => repo.DeleteProduct(It.IsAny<Guid>())).Callback<Guid>(id => _items.RemoveAll(i => i.Id == id));
            _controller = new ProductController(mockRepo.Object);
        }

        [Fact]
        public void DeleteProductTest()
        {
            var id = new Guid("ce2dbb82-6689-487b-9691-0a05ebabce4a");
            var item = _items.FirstOrDefault(i => i.Id == id);
            Assert.NotNull(item);
            var okObjectResult = _controller.Delete(id);
            Assert.IsType<OkResult>(okObjectResult);
            item = _items.FirstOrDefault(i => i.Id == id);
            Assert.Null(item);
        }

        [Fact]
        public void GetProductsTest()
        {
            var okObjectResult = _controller.Get();
            var okResult = Assert.IsType<OkObjectResult>(okObjectResult.Result);
            var items = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public void GetProductTest()
        {
            var id = new Guid("ce2dbb82-6689-487b-9691-0a05ebabce4a");
            var okObjectResult = _controller.Get(id);
            var okResult = Assert.IsType<OkObjectResult>(okObjectResult.Result);
            var item = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(id, item.Id);
        }

        [Fact]
        public void InsertProductTest()
        {
            var product = new Product
            {
                Id = new Guid("d378ff93-dc4b-4bf6-8756-58b6901cd47b"),
                Name = "iPhone X",
                Description = "iPhone X mobile phone",
                Price = 1200
            };

            var createdResponse = _controller.Post(product);
            var response = Assert.IsType<OkObjectResult>(createdResponse);
            var item = Assert.IsType<Product>(response.Value);
            Assert.Equal("iPhone X", item.Name);
        }

        [Fact]
        public void UpdateProductTest()
        {
            var id = new Guid("ce2dbb82-6689-487b-9691-0a05ebabce4a");
            var okObjectResult = _controller.Put(new Product { Id = id, Name = "Samsung Galaxy S10+", Description = "Samsung Galaxy S10+ mobile phone", Price = 1100 });
            Assert.IsType<OkResult>(okObjectResult);
            var item = _items.First(i => i.Id == id);
            Assert.Equal("Samsung Galaxy S10+", item.Name);
            okObjectResult = _controller.Put(null);
            Assert.IsType<NoContentResult>(okObjectResult);
        }
    }
}