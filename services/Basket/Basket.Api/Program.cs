
using Asp.Versioning;
using Basket.Application.GrpcServices;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Core.Repository;
using Basket.Infrastructure.Repository;
using common.logging;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Basket.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //add global authorization filter to all end points 
            var authpolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            builder.Services.AddControllers(config =>
            {
                config.Filters.Add(new AuthorizeFilter(authpolicy));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            //configer authentication and authorization 
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Authority = "https://id-local.eshopping.com:44344";
                    opt.RequireHttpsMetadata = true;

                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://id-local.eshopping.com:44344",
                        ValidateAudience = true,
                        ValidAudience = "Basket",
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    opt.BackchannelHttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                    };

                    opt.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("======Authentication faild");
                            Console.WriteLine($"======exception : {context.Exception.Message}");
                            Console.WriteLine($"======Authority : {opt.Authority}");
                            return Task.CompletedTask;
                        }
                    };
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

            // configer rabbid mq and masstransint 
            builder.Services.AddMassTransit(cfg =>
            {
                cfg.UsingRabbitMq((context, config) =>
                {
                    config.Host(builder.Configuration["EventBusSetting:HostAddress"]);
                });
            });

            builder.Services.AddMassTransitHostedService();


            builder.Services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
            }).AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });

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


                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ShopingCart API",
                    Version = "v2",
                    Description = "API for managing ShopingCart items. v2",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Ahmed Ashraf",
                        Email = "aaboshady59@gmail.com",
                        Url = new Uri("https://ahmedashraf-henna.vercel.app/")
                    }
                });

                //AddApiExplorer now is responsible for
                //filtering the endpoints based on the api version
                //and make the including and grouping,
                //so we don't need to use DocInclusionPredicate anymore

                //options.DocInclusionPredicate((version, apiDesc) =>
                //{
                //    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                //    var versions = methodInfo.DeclaringType?
                //        .GetCustomAttributes(true)
                //        .OfType<ApiVersionAttribute>()
                //        .SelectMany(attr => attr.Versions);
                //    return versions?.Any(v => $"v{v.ToString()}" == version) ?? false;
                //});
            });


            //configer logging
            builder.Host.UseSerilog(Logging.ConfigureLogger);


            var app = builder.Build();

            // BEFORE UseSwagger / routing
            app.Use((ctx, next) =>
            {
                if (ctx.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var p) && !string.IsNullOrEmpty(p))
                    ctx.Request.PathBase = p.ToString();   // e.g., "/catalog"
                return next();
            });

            app.UseSwagger(c =>
            {
                // Make the OpenAPI "servers" base path match the prefix so Try it out uses /catalog/...
                c.PreSerializeFilters.Add((doc, req) =>
                {
                    var prefix = req.Headers["X-Forwarded-Prefix"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(prefix))
                        doc.Servers = new List<Microsoft.OpenApi.Models.OpenApiServer>
            { new() { Url = prefix } };
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Catalog.API v1"); // relative path (no leading '/')
                c.RoutePrefix = "swagger";
            });


            //var nginxPath = "/basket";

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI(
            //        c =>
            //        {
            //            c.SwaggerEndpoint($"{nginxPath}/swagger/v1/swagger.json", "basket api v1");
            //            c.SwaggerEndpoint($"{nginxPath}/swagger/v2/swagger.json", "basket api v2");
            //        }
            //        );
            //}
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
