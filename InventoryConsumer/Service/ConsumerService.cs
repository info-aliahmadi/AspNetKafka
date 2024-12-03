using Confluent.Kafka;

namespace InventoryConsumer.Service
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;

        private readonly ILogger<ConsumerService> _logger;

        public ConsumerService(IConfiguration configuration, ILogger<ConsumerService> logger)
        {
            _logger = logger;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = "InventoryConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("InventoryChanged");

            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessKafkaMessage(stoppingToken);

                Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _consumer.Close();
        }

        public void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                var message = consumeResult.Message.Value;
                var key = consumeResult.Message.Key;

                _logger.LogInformation($"Received inventory update {key}: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Kafka message: {ex.Message}");
            }
        }
    }
}
