using Confluent.Kafka;
using PaymentService.Controllers;

namespace PaymentService.Handlers
{
    public class OrderConsumerHandler : IHostedService
    {
        private readonly ILogger<QrCodeController> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IConsumer<string, string> _consumer;

        public OrderConsumerHandler(ILogger<QrCodeController> logger, IConsumer<string, string> consumer)
        {
            _logger = logger;
            _cancellationTokenSource = new CancellationTokenSource();
            _consumer = consumer;
            _consumer.Subscribe("drink-order-topic");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var cancelToken = new CancellationTokenSource();
            try
            {
                while (true)
                {
                    var consumer = _consumer.Consume(cancelToken.Token);
                    Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}");
                }
            }
            catch (Exception)
            {
                _consumer.Close();
            }
            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
