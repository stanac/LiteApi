using LiteApi.OpenApiSample.Models;
using System;
using System.Collections.Generic;

namespace LiteApi.OpenApiSample.DataAccess
{
    public interface IOrdersDataAccess
    {
        IEnumerable<Order> GetAll();
        Order GetById(Guid id);
        Order Add(Order model);
    }
}
