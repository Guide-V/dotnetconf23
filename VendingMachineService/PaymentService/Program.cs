using Confluent.Kafka;
using PaymentService.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = builder.Configuration.GetValue<string>("KafkaConsumerConfig:BootstrapServers"),
    GroupId = "test-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};
builder.Services.AddSingleton(consumerConfig); // Register Kafka producer configuration
builder.Services.AddSingleton<IConsumer<string, string>>(new ConsumerBuilder<string, string>(consumerConfig).Build());

var producerConfig = new ProducerConfig
{
    BootstrapServers = builder.Configuration.GetValue<string>("KafkaProducerConfig:BootstrapServers")
};

builder.Services.AddSingleton(producerConfig); // Register Kafka producer configuration
builder.Services.AddSingleton<IProducer<string, string>>(new ProducerBuilder<string, string>(producerConfig).Build());

builder.Services.AddHostedService<OrderConsumerHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
