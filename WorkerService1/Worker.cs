using Azure.Identity;
using Confluent.Kafka;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ExternalConnectors;
using Newtonsoft.Json;
using WorkerService1.BL;
using WorkerService1.controllers;
using WorkerService1.Modal;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumer<Ignore, string> consumer;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            ConsumerConfig config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "email-consumers",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            

            consumer.Subscribe("Notifier");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // waiting for kafka message event
                    ConsumeResult<Ignore,string> consumeResult = consumer.Consume();

                    //sending email 
                    EmailNotifier.NotifyError(consumeResult);

                    // Send Teams message
                    //TeamsNotifier.Notify(consumeResult);
                    //await SendTeamsMessage(teamsMessage);

                    // logging info
                    _logger.LogInformation($"Consumed message '{consumeResult?.Message.Value}' at: '{consumeResult.Offset}'");

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing Teams message");
                    await Task.Delay(1000, stoppingToken);
                }
            }

            //return Task.CompletedTask;
        }
    }
}
