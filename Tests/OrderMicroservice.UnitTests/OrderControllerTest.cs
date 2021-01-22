using CartMicroservice.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using OrderModel;
using OrderRepository;

namespace OrderMicroservice.UnitTests
{
    public class OrderControllerTest
    {
        private readonly OrderController _controller;

        private readonly List<Order> _orders = new List<Order>
        {
            new Order
            {
                Id = new Guid("6bd19484-2237-4319-a690-510e4f2258d8"),
                UserId = new Guid("f5dd5ea6-ac9c-4dc3-86c3-090853945167"),
                OrderItems = new List<OrderItem>
                {
                    new OrderItem{ ProductId= new Guid("ce2dbb82-6689-487b-9691-0a05ebabce4a"), ProductName = "Samsung Galaxy S10", ProductPrice = 1050, Quantity = 1},
                    new OrderItem{ ProductId= new Guid("13b87ba8-f542-441c-bc2c-db32fb01ec3f"), ProductName = "Samsung Galaxy S9", ProductPrice = 700, Quantity = 1}
                }
            }
        };

        public OrderControllerTest()
        {
            var mockRepo = new Mock<IOrderRepository>();

            mockRepo.Setup(repo => repo.GetOrderItems(It.IsAny<Guid>())).Returns<Guid>(userId =>
            {
                var order = _orders.FirstOrDefault(c => c.UserId == userId);

                if (order != null)
                {
                    return order.OrderItems;
                }

                return new List<OrderItem>();
            });

            mockRepo.Setup(repo => repo.InsertOrderItem(It.IsAny<Guid>(), It.IsAny<OrderItem>())).Callback<Guid, OrderItem>((userId, cartItem) =>
            {
                var cart = _orders.FirstOrDefault(c => c.UserId == userId);

                if (cart != null)
                {
                    var ci = cart.OrderItems.FirstOrDefault(i => i.ProductId == cartItem.ProductId);

                    if (ci == null)
                    {
                        cart.OrderItems.Add(cartItem);
                    }
                    else
                    {
                        ci.Quantity++;
                    }
                }
            });

            _controller = new OrderController(mockRepo.Object);
        }

        [Fact]
        public void GetOrderItemsTest()
        {
            var userId = new Guid("f5dd5ea6-ac9c-4dc3-86c3-090853945167");
            var okObjectResult = _controller.Get(userId);
            var okResult = Assert.IsType<OkObjectResult>(okObjectResult.Result);
            var items = Assert.IsType<List<OrderItem>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public void InsertOrderItemTest()
        {
            var userId = new Guid("f5dd5ea6-ac9c-4dc3-86c3-090853945167");
            var orderItem = new OrderItem { ProductId = new Guid("d378ff93-dc4b-4bf6-8756-58b6901cd47b"), ProductName = "iPhone X", ProductPrice = 1200, Quantity = 3 };
            var okResult = _controller.Post(userId, orderItem);
            Assert.IsType<OkResult>(okResult);
            var cart = _orders.First(c => c.UserId == userId);
            var ci = cart.OrderItems.FirstOrDefault(i => i.ProductId == orderItem.ProductId);
            Assert.NotNull(ci);
            Assert.Equal(3, ci.Quantity);
            okResult = _controller.Post(userId, orderItem);
            Assert.IsType<OkResult>(okResult);
            Assert.Equal(4, ci.Quantity);
        }
    }
}