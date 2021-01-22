using ProductModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _collection;

        public ProductRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<Product>(Product.DocumentName);
        }

        public void DeleteProduct(Guid id) =>
            _collection.DeleteOne(c => c.Id == id);

        public Product GetProduct(Guid id) =>
            _collection.Find(c => c.Id == id).FirstOrDefault();

        public List<Product> GetProducts() =>
                            _collection.Find(FilterDefinition<Product>.Empty).ToList();

        public void InsertProduct(Product product) =>
            _collection.InsertOne(product);

        public void UpdateProduct(Product product) =>
            _collection.UpdateOne(c => c.Id == product.Id, Builders<Product>.Update
                                                                     .Set(c => c.Name, product.Name)
                                                                     .Set(c => c.Description, product.Description)
                                                                     .Set(c => c.Price, product.Price));
    }
}