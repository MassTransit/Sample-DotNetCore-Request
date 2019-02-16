using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Sample_RequestResponse
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var builder = new HostBuilder()
        .ConfigureServices((hostContext, services) =>
        {
          services.AddHostedService<MessageQueueService>();
        });

      await builder
        .RunConsoleAsync();
    }
  }
}