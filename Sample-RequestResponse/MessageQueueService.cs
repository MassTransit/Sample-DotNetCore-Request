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
    private readonly IBusControl _bus;

    public MessageQueueService()
    {
      _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
          var host = cfg.Host(new Uri("rabbitmq://localhost:5672/"), h => {
            h.Username("rabbitmq");
            h.Password("rabbitmq");
          });

          cfg.ReceiveEndpoint(host, "order-service", e =>
          {
            e.Handler<SubmitOrder>(context => context.RespondAsync<OrderAccepted>(new
            {
              context.Message.OrderId
            }));
          });
        });
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      return _bus.StartAsync();
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.WhenAll(base.StopAsync(cancellationToken), _bus.StopAsync());
    }
  }
}
