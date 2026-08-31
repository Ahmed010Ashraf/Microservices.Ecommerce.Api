
using Ocelot.DependencyInjection;
using System.Threading.Tasks;


namespace OcelotApiGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


            //add ocelot configuration
            builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            builder.Services.AddOcelot(builder.Configuration);


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            app.UseRouting();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello from Ocelot API Gateway");
                });
            });
            app.UseAuthorization();


            app.MapControllers();

           await app.RunAsync();
        }
    }
}
