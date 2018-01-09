using System;
using System.Threading.Tasks;
using MassTransit;
using MessageContracts;

namespace Sample_RequestResponse
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h => { });

                cfg.ReceiveEndpoint(host, "order-service", e =>
                {
                    e.Handler<SubmitOrder>(context => context.RespondAsync<OrderAccepted>(new
                    {
                        context.Message.OrderId
                    }));
                });
            });

            await bus.StartAsync();
            try
            {
                Console.WriteLine("Working....");

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                await bus.StopAsync();
            }
        }
    }
}