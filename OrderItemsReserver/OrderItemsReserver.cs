using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;
using System.Text;

namespace OrderItemsReserver
{
    public class OrderItemsReserver
    {
        // [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] OrderDto order

        // await using (var writer = binder.Bind<TextWriter>(new BlobAttribute($"order-items/{order.BuyerId}", FileAccess.Write)))
        // {
        //    writer.Write("Hello World!");
        // };
        // [Blob("order-items/{buyerId}.json", FileAccess.Write)] Stream outputBlob
        // [Blob("order-items/{rand-guid}.json", FileAccess.Write, Connection = "dsadsad")] TextWriter writer

        // TODO: Add fluent validation API https://tech.playgokids.com/azure-functions-fluent-validation/

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        // TODO: Make configuration strong typed 
        public OrderItemsReserver(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        [FunctionName("OrderItemsReserver")]
        public async Task Run([ServiceBusTrigger("reservedorderitemsqueue")] ServiceBusReceivedMessage message)
        {
            //var order = message.Body.ToObjectFromJson<OrderDto>();

            try
            {
                var blobServiceClient = new BlobServiceClient(_configuration["AzureWebJobsBlobStorage"],
                    new BlobClientOptions
                    {
                        Retry = { MaxRetries = 3 }
                    });

                // TODO: Extract queue name to configuration
                var containerClient = blobServiceClient.GetBlobContainerClient("reserved-order-items");
                await containerClient.CreateIfNotExistsAsync();
                await containerClient.UploadBlobAsync($"{Guid.NewGuid()}.json", message.Body);

            }
            catch (Exception e)
            {
                var uri = new Uri(_configuration["AzureWebJobsLogicApp"]);
                var stringContent = new StringContent(
                    message.Body.ToString(),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json);
                await _httpClient.PostAsync(uri, stringContent);

                // TODO: add logging of exception
            }
        }
    }
}
