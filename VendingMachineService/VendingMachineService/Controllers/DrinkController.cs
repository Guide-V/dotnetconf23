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
        public IActionResult Post(DrinkOrder drinkOrderDto)
        {
            var drinkOrder = new DrinkOrder
            {
                Id = drinkOrderDto.Id,
                Price = drinkOrderDto.Price,
                CreatedDate = DateTime.Now,
                HasStraw = drinkOrderDto.HasStraw
            };

            try
            {
                var orderJson = JsonSerializer.Serialize(drinkOrder);

                // Produce the order message to the "drink-order-topic"
                _kafkaProducer.Produce("drink-order-topic", new Message<string, string>
                {
                    Key = null,
                    Value = orderJson
                });

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
