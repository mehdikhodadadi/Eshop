using OrderModel;
using System;
using System.Collections.Generic;

namespace OrderRepository
{
    public interface IOrderRepository
    {
        List<OrderItem> GetOrderItems(Guid userId);

        void InsertOrderItem(Guid userId, OrderItem orderItem);
    }
}