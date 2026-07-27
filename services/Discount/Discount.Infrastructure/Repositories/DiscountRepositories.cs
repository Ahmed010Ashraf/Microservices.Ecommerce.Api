using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepositories : IDiscountRepository
    {
        private readonly IConfiguration _Config;

        public DiscountRepositories(IConfiguration config)
        {
            _Config = config;
        }

        public async Task<Coupon> GetDiscount(string ProductName)
        {
            await using var connection = new NpgsqlConnection(_Config.GetValue<string>("DatabaseSetting:ConnectionString"));

            var query = "SELECT * FROM Coupon WHERE ProductName = @ProductName";

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(query, new { ProductName = ProductName});

            if (coupon != null) { 
            return coupon;
            }
            return new Coupon() { 
                ProductName = ProductName,
                Amount = 0,
                Description = "there is no coupon for this product"
            };
        }

        public async Task<bool> CreateDiscount(Coupon Coupon)
        {
            await using var connection = new NpgsqlConnection(_Config.GetValue<string>("DatabaseSetting:ConnectionString"));

            var affected = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName , Description , Amount) VALUES (@ProductName , @Description , @Amount)", new
            {
                ProductName = Coupon.ProductName,
                Amount = Coupon.Amount,
                Description = Coupon.Description,
            });


            return affected > 0;
        }

        public async Task<bool> UpdateDiscount(Coupon Coupon)
        {
            await using var connection = new NpgsqlConnection(_Config.GetValue<string>("DatabaseSetting:ConnectionString"));

            var affected = await connection.ExecuteAsync("""
                                                         UPDATE Coupon 
                                                         SET
                                                           ProductName = @ProductName ,
                                                           Description = @Description,
                                                           Amount = @Amount
                                                        WHERE Id = @Id
                """, new 
            {
                ProductName = Coupon.ProductName,
                Amount = Coupon.Amount,
                Description = Coupon.Description,
                Id = Coupon.Id
            });


            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string ProductName)
        {
            await using var connection = new NpgsqlConnection(_Config.GetValue<string>("DatabaseSetting:ConnectionString"));

            var affected = await connection.ExecuteAsync("""
                                                         DELETE FROM Coupon
                                                         WHERE ProductName = @ProductName
                """, new
            {

                ProductName = ProductName
            });


            return affected > 0;
        }

     
       
    }
}
