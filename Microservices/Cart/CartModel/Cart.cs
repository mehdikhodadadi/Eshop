using System;
using System.Collections.Generic;

namespace CartModel
{
    public class Cart
    {
        public static readonly string DocumentName = "carts";

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}