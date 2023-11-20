using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var producerConfig = new ProducerConfig
{
    BootstrapServers = builder.Configuration.GetValue<string>("KafkaProducerConfig:BootstrapServers")
};

builder.Services.AddSingleton(producerConfig); // Register Kafka producer configuration
builder.Services.AddSingleton<IProducer<string, string>>(new ProducerBuilder<string, string>(producerConfig).Build());
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
