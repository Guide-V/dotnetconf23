using Confluent.Kafka;

namespace PaymentService.Handlers
{
    public class OrderConsumerHandler : IHostedService
    {
        private readonly ILogger<OrderConsumerHandler> _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IConsumer<string, string> _consumer;
        private readonly IProducer<string, string> _producer;

        public OrderConsumerHandler(ILogger<OrderConsumerHandler> logger, IConsumer<string, string> consumer, IProducer<string, string> producer)
        {
            _logger = logger;
            _cancellationTokenSource = new CancellationTokenSource();
            _consumer = consumer;
            _consumer.Subscribe("drink-order-topic");
            _producer = producer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var cancelToken = new CancellationTokenSource();
            try
            {
                while (true)
                {
                    var consumer = _consumer.Consume(cancelToken.Token);
                    //Generate payment url
                    var paymentUrl = $"orderId: {consumer.Message.Value}, bank-url-or-qr-code";
                    
                    //Put payment url into payment topic
                    _producer.Produce("payment-topic", new Message<string, string>
                    {
                        Key = null,
                        Value = paymentUrl
                    });

                    //or put it in a db?
                    //or whatever...

                    Console.WriteLine($"Message: {consumer.Message.Value} received from {consumer.TopicPartitionOffset}, and created paymenturl {paymentUrl}");
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
