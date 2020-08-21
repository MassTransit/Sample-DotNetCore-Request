using System;
using MassTransit;
using MessageContracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) => cfg.Host("localhost"));

                var timeout = TimeSpan.FromSeconds(10);
                var serviceAddress = new Uri("rabbitmq://localhost/order-service");

                x.AddRequestClient<SubmitOrder>(serviceAddress, timeout);
            });
            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}