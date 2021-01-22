using OrderModel;
using OrderRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CartMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderItem>> Get([FromQuery(Name = "u")] Guid userId)
        {
            var cartItems = _orderRepository.GetOrderItems(userId);
            return Ok(cartItems);
        }

        [HttpPost]
        public ActionResult Post([FromQuery(Name = "u")] Guid userId, [FromBody] OrderItem orderItem)
        {
            _orderRepository.InsertOrderItem(userId, orderItem);
            return Ok();
        }
    }
}