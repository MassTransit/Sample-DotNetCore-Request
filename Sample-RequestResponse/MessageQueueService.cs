using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MessageContracts;
using Microsoft.Extensions.Hosting;

namespace Sample_RequestResponse
{
    public class MessageQueueService : BackgroundService
    {
        readonly IBusControl _bus;

        public MessageQueueService()
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://localhost/"), h => { });

                cfg.ReceiveEndpoint("order-service", e =>
                {
                    e.Handler<SubmitOrder>(context =>
                    {
                        Console.WriteLine("Order: {0}", context.Message.OrderId);

                        return context.RespondAsync<OrderAccepted>(new
                        {
                            context.Message.OrderId
                        });
                    });
                });
            });
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _bus.StartAsync(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(base.StopAsync(cancellationToken), _bus.StopAsync(cancellationToken));
        }
    }
}