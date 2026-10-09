
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Reposatories;
using Catalog.Infrastructure.Data.Context;
using Catalog.Infrastructure.Reposatory;
using common.logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
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
            //add global authorization filter to all end points 
            var authpolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            builder.Services.AddControllers(config =>
            {
                config.Filters.Add(new AuthorizeFilter(authpolicy));
            });
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

            //configer authentication and authorization 
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Authority = "http://identityserver:9011";
                    opt.RequireHttpsMetadata = false;

                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "http://identityserver:9011",
                        ValidateAudience = true,
                        ValidAudience = "Catalog",
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

            //configer logging
            builder.Host.UseSerilog(Logging.ConfigureLogger);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
