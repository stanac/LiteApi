using System.Collections.Generic;

namespace LiteApi.OpenApiDemo.Models
{
    public class Order
    {
        public Orderer Orderer { get; set; }
        public string DeliveryAddress { get; set; }
        public List<Book> Books { get; set; }
        public OrderStatus Status { get; set; }
    }
}
