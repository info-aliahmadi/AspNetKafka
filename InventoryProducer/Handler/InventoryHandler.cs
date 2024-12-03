using InventoryProducer.Model;
using InventoryProducer.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace InventoryProducer.Handler
{
    public static class InventoryHandler
    {
        public static IList<InventoryRequest> InventoryRequests = new List<InventoryRequest>()
        {
            new InventoryRequest()
            {
                Id = 1,
                ProductId = "Product 1",
                Quantity = 5
            },
            new InventoryRequest()
            {
                Id = 2,
                ProductId = "Product 2",
                Quantity = 6
            },
            new InventoryRequest()
            {
                Id = 3,
                ProductId = "Product 3",
                Quantity = 8
            },
            new InventoryRequest()
            {
                Id = 4,
                ProductId = "Product 4",
                Quantity = 55
            },
            new InventoryRequest()
            {
                Id = 5,
                ProductId = "Product 5",
                Quantity = 53
            },
            new InventoryRequest()
            {
                Id = 6,
                ProductId = "Product 6",
                Quantity = 15
            },
        };
        public static async Task<IResult> UpdateInventory(ProducerService producerService, InventoryRequest request)
        {
            var inve = InventoryRequests.First(x => x.Id == request.Id);
            inve.Quantity = request.Quantity;

            var message = JsonSerializer.Serialize(inve);

            await producerService.ProduceAsync("InventoryChanged", message);

            return Results.Ok(inve);
        }
        public static IResult GetInventories()
        {
            return Results.Ok(InventoryRequests);
        }
    }
}
