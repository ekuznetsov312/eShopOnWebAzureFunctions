using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using OrderItemsDelivery.Models;

namespace OrderItemsDelivery
{
    public static class OrderItemsDelivery
    {
        [FunctionName("OrderItemsDelivery")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            OrderDto order,
            [CosmosDB(
                databaseName: "orders",
                containerName: "delivery",
                Connection = "CosmosDbConnectionString",
                CreateIfNotExists = true,
                PartitionKey = "/delivery")] IAsyncCollector<dynamic> documentsOut)
        {
            await documentsOut.AddAsync(new
            {
                id = System.Guid.NewGuid().ToString(),
                order
            });

            return new OkResult();
        }
    }
}
