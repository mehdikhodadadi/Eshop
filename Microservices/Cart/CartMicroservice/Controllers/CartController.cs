using CartModel;
using CartRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CartMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpDelete]
        public ActionResult Delete([FromQuery(Name = "u")] Guid userId, [FromQuery(Name = "p")] Guid productId)
        {
            _cartRepository.DeleteCartItem(userId, productId);
            return Ok();
        }

        [HttpGet]
        public ActionResult<IEnumerable<CartItem>> Get([FromQuery(Name = "u")] Guid userId)
        {
            var cartItems = _cartRepository.GetCartItems(userId);
            return Ok(cartItems);
        }

        [HttpPost]
        public ActionResult Post([FromQuery(Name = "u")] Guid userId, [FromBody] CartItem cartItem)
        {
            _cartRepository.InsertCartItem(userId, cartItem);
            return Ok();
        }

        [HttpPut]
        public ActionResult Put([FromQuery(Name = "u")] Guid userId, [FromBody] CartItem cartItem)
        {
            _cartRepository.UpdateCartItem(userId, cartItem);
            return Ok();
        }
    }
}