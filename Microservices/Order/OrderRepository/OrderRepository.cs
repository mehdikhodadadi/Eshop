using OrderModel;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _collection;

        public OrderRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<Order>(Order.DocumentName);
        }

        public List<OrderItem> GetOrderItems(Guid userId)
        {
            var order = _collection.Find(c => c.UserId == userId)
                                   .FirstOrDefault();

            if (order != null)
            {
                return order.OrderItems;
            }

            return new List<OrderItem>();
        }

        public void InsertOrderItem(Guid userId, OrderItem orderItem)
        {
            var order = _collection.Find(c => c.UserId == userId)
                                   .FirstOrDefault();

            if (order == null)
            {
                order = new Order { UserId = userId, OrderItems = new List<OrderItem> { orderItem } };
                _collection.InsertOne(order);
            }
            else
            {
                var orderItemInDb = order.OrderItems.FirstOrDefault(ci => ci.ProductId == orderItem.ProductId);

                if (orderItemInDb == null)
                {
                    order.OrderItems.Add(orderItem);
                }
                else
                {
                    orderItemInDb.Quantity++;
                }

                var update = Builders<Order>.Update
                                            .Set(c => c.OrderItems, order.OrderItems);

                _collection.UpdateOne(c => c.UserId == userId, update);
            }
        }
    }
}