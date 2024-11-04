using API.Entities.OrderAggregate;

namespace API.Data.DTO
{
    public class CreateOrderDto
    {
        public bool SaveAddress {get; set;}
        public ShippingAddress ShippingAddress { get; set; }
    }
}