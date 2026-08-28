
using common.logging;
using Discount.Api.Services;
using Discount.Application.Command;
using Discount.Application.Mapper;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extention;
using Discount.Infrastructure.Repositories;
using Serilog;
using System.Reflection;

namespace Discount.Api
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
            builder.Services.AddSwaggerGen();


            builder.Services.AddAutoMapper(ctf => { }, typeof(DiscountProfile).Assembly);
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.RegisterServicesFromAssembly(typeof(CreateDiscountCommand).Assembly);
            });

            builder.Services.AddScoped<IDiscountRepository , DiscountRepositories>();
            builder.Services.AddGrpc();

            var app = builder.Build();


            //configer logging
            builder.Host.UseSerilog(Logging.ConfigureLogger);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Migration<Program>();

            app.UseRouting();

            app.UseEndpoints(endpoints => {

                endpoints.MapGrpcService<DiscountService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communitation with gRPC service should be by grpc client");
                });

            });

            //app.MapGrpcService<DiscountService>();
            //app.MapGet("/", async context =>
            //{
            //    await context.Response.WriteAsync("Communitation with gRPC service should be by grpc client");
            //});

            app.Run();
        }
    }
}
