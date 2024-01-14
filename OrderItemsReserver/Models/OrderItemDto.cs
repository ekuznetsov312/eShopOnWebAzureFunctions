using System.Text.Json.Serialization;

namespace OrderItemsReserver.Models
{
    public class OrderItemDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}