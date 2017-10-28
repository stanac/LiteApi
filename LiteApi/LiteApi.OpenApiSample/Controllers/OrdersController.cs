using LiteApi.OpenApiSample.DataAccess;
using System;
using System.Collections.Generic;
using LiteApi.OpenApiSample.Models;
using LiteApi.Attributes;
using LiteApi.OpenApi.Attributes;

namespace LiteApi.OpenApiSample.Controllers
{
    [Restful]
    public class OrdersController : LiteController
    {
        private IOrdersDataAccess _data;

        public OrdersController(IOrdersDataAccess data)
        {
            _data = data;
        }

        [HttpPost, OpenApiOperation("AddOrder", 200)]
        public Order Add(Order model)
        {
            return _data.Add(model);
        }

        [HttpGet, OpenApiOperation("GetAllOrders", 200)]

        public IEnumerable<Order> GetAll()
        {
            return _data.GetAll();
        }

        [HttpGet, ActionRoute("/{id}"), OpenApiOperation("GetOrderById", 200, 404)]
        public Order GetById(Guid id)
        {
            var model = _data.GetById(id);
            if (model == null) SetResponseStatusCode(404);
            return model;
        }
    }
}
