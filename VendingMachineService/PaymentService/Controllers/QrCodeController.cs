using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QrCodeController : Controller
    {
        private readonly ILogger<QrCodeController> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IConsumer<string,string> _consumer;

        public QrCodeController(ILogger<QrCodeController> logger, IConsumer<string, string> consumer)
        {
            _logger = logger;
            _consumer = consumer;
            _cancellationTokenSource = new CancellationTokenSource();

            _consumer.Subscribe("drink-order-topic");
        }

        [HttpGet]
        public IActionResult Get() 
        {
            return Ok("Controller is working!");
        }

        [HttpPost(Name = "GenerateQr")]
        public Task Post()
        {
            try
            {
                while (true)
                {
                    var result = _consumer.Consume(_cancellationTokenSource.Token);

                    // Process the received message
                    var message = result.Message.Value;

                    // You can process the message here or pass it to a service for further processing

                    Console.WriteLine($"Received message: {message}");
                }
            }
            catch (Exception)
            {
                // Handle the cancellation gracefully
                return Task.CompletedTask;
            }
            finally { _consumer.Close(); }
        }

/*        [HttpPost("stop")]
        public IActionResult PostStop()
        {
            // Signal the cancellation to stop the consumer
            _cancellationTokenSource.Cancel();

            // Close the Kafka consumer
            _consumer.Close();

            return Ok("Consumer stopping...");
        }*/

    }
}
