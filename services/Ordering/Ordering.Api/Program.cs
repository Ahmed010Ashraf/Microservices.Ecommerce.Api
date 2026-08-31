
using common.logging;
using EventBus.Messages.Common;
using MassTransit;
using Ordering.Api.EventBusConsumer;
using Ordering.Api.Extentions;
using Ordering.Application.Extentions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extentions;
using Serilog;

namespace Ordering.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
            });


            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Order API",
                    Version = "v1",
                    Description = "API for managing Order items.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Ahmed Ashraf",
                        Email = "aaboshady59@gmail.com",
                        Url = new Uri("https://ahmedashraf-henna.vercel.app/")
                    }
                });
            });


            builder.Services.AddApplicationServices();
            builder.Services.AddInfraService(builder.Configuration);


            //configer rabbidmq
            builder.Services.AddScoped<BasketOrderConsumer>();
            builder.Services.AddScoped<BasketOrderConsumerV2>();
            builder.Services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<BasketOrderConsumer>();
                cfg.AddConsumer<BasketOrderConsumerV2>();
                cfg.UsingRabbitMq((context, config) =>
                {
                    config.Host(builder.Configuration["EventBusSetting:HostAddress"]);

                    config.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
                    {
                        c.ConfigureConsumer<BasketOrderConsumer>(context);
                    });

                    config.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueueV2, c =>
                    {
                        c.ConfigureConsumer<BasketOrderConsumerV2>(context);
                    });
                });
            });

            builder.Services.AddMassTransitHostedService();

            //configer logging
            builder.Host.UseSerilog(Logging.ConfigureLogger);

            var app = builder.Build();

            app.MigrateDatabase<OrderContext>((context, services) =>
            {
                var logger = services.GetService<ILogger<OrderContextSeed>>();
                OrderContextSeed.SeedAsync(context, logger).Wait();
                
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
