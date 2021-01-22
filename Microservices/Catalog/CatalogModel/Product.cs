using System;

namespace ProductModel
{
    public class Product
    {
        public static readonly string DocumentName = "products";

        public string Description { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}