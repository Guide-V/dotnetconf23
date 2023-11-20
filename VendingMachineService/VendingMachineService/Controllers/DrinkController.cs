using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using VendingMachineService.Models;
using System.Text.Json;

namespace VendingMachineService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrinkController : Controller
    {
        private readonly ILogger<DrinkController> _logger;
        private readonly IProducer<string, string> _kafkaProducer;

        public DrinkController(ILogger<DrinkController> logger, IProducer<string, string> kafkaProducer)
        {
            _logger = logger;
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost(Name = "OrderDrink")]
        public IActionResult Post(DrinkOrder drinkOrderPTO)
        {
            var drinkOrder = new DrinkOrder
            {
                Id = drinkOrderPTO.Id,
                Price = drinkOrderPTO.Price,
                CreatedDate = DateTime.Now,
                HasStraw = drinkOrderPTO.HasStraw
            };

            //put order into Kafka topic: drink-order-topic
            try
            {

                // Serialize the order as JSON (assuming you're using JSON)
                var orderJson = JsonSerializer.Serialize(drinkOrder);

                    // Produce the order message to the "drink-order-topic"
                    _kafkaProducer.Produce("drink-order-topic", new Message<string, string>
                    {
                        Key = null, // You can specify a key if needed
                        Value = orderJson
                    });
                

                // Optionally, you can log or handle successful message production here

                return Ok(drinkOrder);
            }
            catch (ProduceException<string, string> ex)
            {
                // Handle any Kafka producer errors here
                // You can log or throw an exception as needed
                _logger.LogError($"Error producing message: {ex.Error.Reason}");
                return StatusCode(500, "Error producing message");
            }

        }
    }
}
