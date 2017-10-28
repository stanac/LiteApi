using System;
using System.Collections.Generic;
using System.Linq;
using LiteApi.OpenApiSample.Models;

namespace LiteApi.OpenApiSample.DataAccess
{
    public class OrdersDataAccess : IOrdersDataAccess
    {
        private static List<Order> _orders;

        static OrdersDataAccess()
        {
            _orders = new List<Order>();
            _orders.AddRange(SampleData.Orders);
        }

        public Order Add(Order model)
        {
            model.Id = Guid.NewGuid();
            _orders.Add(model);
            return model;
        }

        public IEnumerable<Order> GetAll()
        {
            return _orders;
        }

        public Order GetById(Guid id)
        {
            return _orders.FirstOrDefault(x => x.Id == id);
        }
    }
}
