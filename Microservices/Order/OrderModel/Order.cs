using System;
using System.Collections.Generic;

namespace OrderModel
{
    public class Order
    {
        public static readonly string DocumentName = "orders";

        public Guid Id { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Guid UserId { get; set; }
    }
}