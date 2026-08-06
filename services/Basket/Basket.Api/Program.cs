
using Basket.Application.GrpcServices;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Repository;
using Basket.Infrastructure.Repository;
using Discount.Grpc.Protos;
using System.Reflection;

namespace Basket.Api
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
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ShopingCart API",
                    Version = "v1",
                    Description = "API for managing ShopingCart items.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Ahmed Ashraf",
                        Email = "aaboshady59@gmail.com",
                        Url = new Uri("https://ahmedashraf-henna.vercel.app/")
                    }
                });
            });



            //add custom configrations
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            //configer redis
            builder.Services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = builder.Configuration.GetValue<string>("CacheSetting:ConnectionString");
            });

            builder.Services.AddAutoMapper(ctf => { }, typeof(ShopingCartProfile).Assembly);
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.RegisterServicesFromAssembly(typeof(GetBasketByUserNameQuery).Assembly);
            });

            //register grpc 
            builder.Services.AddScoped<DiscountGrpcService>();
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(
                cfg => cfg.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"])
                );

            builder.Services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
