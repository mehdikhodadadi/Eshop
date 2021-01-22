using System;

namespace OrderModel
{
    public class OrderItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}