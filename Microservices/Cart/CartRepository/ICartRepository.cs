using CartModel;
using System;
using System.Collections.Generic;

namespace CartRepository
{
    public interface ICartRepository
    {
        void DeleteCartItem(Guid userId, Guid productId);

        List<CartItem> GetCartItems(Guid userId);

        void InsertCartItem(Guid userId, CartItem cartItem);

        void UpdateCartItem(Guid userId, CartItem cartItem);
    }
}