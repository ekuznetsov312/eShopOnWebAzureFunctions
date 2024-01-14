using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OrderItemsReserver.Models
{
    public class OrderDto
    {
        [JsonPropertyName("buyer_id")]
        public string BuyerId { get; set; }

        [JsonPropertyName("order_items")]
        public List<OrderItemDto> OrderItems { get; set; }
    }
}