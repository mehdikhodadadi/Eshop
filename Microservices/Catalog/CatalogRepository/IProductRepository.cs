using ProductModel;
using System;
using System.Collections.Generic;

namespace ProductRepository
{
    public interface IProductRepository
    {
        void DeleteProduct(Guid id);

        Product GetProduct(Guid id);

        List<Product> GetProducts();

        void InsertProduct(Product product);

        void UpdateProduct(Product product);
    }
}