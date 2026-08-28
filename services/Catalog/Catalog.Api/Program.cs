
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Reposatories;
using Catalog.Infrastructure.Data.Context;
using Catalog.Infrastructure.Reposatory;
using common.logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace Catalog.Api
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
                    Title = "Catalog API",
                    Version = "v1",
                    Description = "API for managing catalog items.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    { 
                        Name = "Ahmed Ashraf",
                        Email = "aaboshady59@gmail.com",
                        Url = new Uri("https://ahmedashraf-henna.vercel.app/")
                    }
                });
            });


            builder.Services.AddScoped<IProductReposatory, ProductReposatory>();
            builder.Services.AddScoped<IBrandReposatory, ProductReposatory>();
            builder.Services.AddScoped<ITypeReposatory, ProductReposatory>();
            builder.Services.AddScoped<ICatalogContext, CatalogContext>();

            builder.Services.AddAutoMapper(ctf=> { },typeof(ProductProfile).Assembly);
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.RegisterServicesFromAssembly(typeof(GetProductByIdQuery).Assembly);
            });

            builder.Services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
            });

            //configer logging
            builder.Host.UseSerilog(Logging.ConfigureLogger);

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
