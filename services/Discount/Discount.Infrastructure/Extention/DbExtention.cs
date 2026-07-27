using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Extention
{
    public static class DbExtention
    {
        public static IHost Migration<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateAsyncScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("db migration started");
                    ApplyMigrations(config);
                    logger.LogInformation("db migration ended");
            }
                catch (Exception ex)
                {
                    logger.LogError(ex, "can not apply migtations");
                    throw;
                }
            }

          

                return host;
        }

        private static void ApplyMigrations(IConfiguration config) {

            int retry = 5;
            while (retry > 0) {
                try
                {
                    var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSetting:ConnectionString"));
                    var command = new NpgsqlCommand()
                    {
                        Connection = connection
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon;";
                    command.ExecuteNonQuery();
                    command.CommandText = @"CREATE TABLE Coupon 
                                                    (Id SERIAL PRIMARY KEY , 
                                                      ProductName VARCHAR(500) NOT NULL ,
                                                       Description TEXT , 
                                                       Amount INT);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon VALUES ('Egypt Adidas Quick Force Indoor Badminton Shoes' ,'Adidas' , 500);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO Coupon VALUES ('PowerFit 19 FH Rubber Spike Cricket Shoes' ,'PowerFit' , 600);";
                    command.ExecuteNonQuery();
                    
                    break;

                }
                catch (Exception ex) { 
                    retry--;
                if(retry == 0)
                    {
                        throw;
                    }
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
