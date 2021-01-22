using CartModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CartRepository
{
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Cart> _collection;

        public CartRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<Cart>(Cart.DocumentName);
        }

        public void DeleteCartItem(Guid userId, Guid productId)
        {
            var cart = _collection.Find(c => c.UserId == userId)
                                  .FirstOrDefault();

            if (cart != null)
            {
                cart.CartItems.RemoveAll(ci => ci.ProductId == productId);

                var update = Builders<Cart>.Update
                                           .Set(c => c.CartItems, cart.CartItems);

                _collection.UpdateOne(c => c.UserId == userId, update);
            }
        }

        public List<CartItem> GetCartItems(Guid userId)
        {
            var cart = _collection.Find(c => c.UserId == userId)
                                  .FirstOrDefault();

            if (cart != null)
            {
                return cart.CartItems;
            }

            return new List<CartItem>();
        }

        public void InsertCartItem(Guid userId, CartItem cartItem)
        {
            var cart = _collection.Find(c => c.UserId == userId)
                                  .FirstOrDefault();

            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem> { cartItem } };
                _collection.InsertOne(cart);
            }
            else
            {
                var cartItemInDb = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItem.ProductId);

                if (cartItemInDb == null)
                {
                    cart.CartItems.Add(cartItem);
                }
                else
                {
                    cartItemInDb.Quantity++;
                }

                var update = Builders<Cart>.Update
                                           .Set(c => c.CartItems, cart.CartItems);

                _collection.UpdateOne(c => c.UserId == userId, update);
            }
        }

        public void UpdateCartItem(Guid userId, CartItem cartItem)
        {
            var cart = _collection.Find(c => c.UserId == userId)
                                  .FirstOrDefault();

            if (cart != null)
            {
                cart.CartItems.RemoveAll(ci => ci.ProductId == cartItem.ProductId);
                cart.CartItems.Add(cartItem);

                var update = Builders<Cart>.Update
                                           .Set(c => c.CartItems, cart.CartItems);

                _collection.UpdateOne(c => c.UserId == userId, update);
            }
        }
    }
}