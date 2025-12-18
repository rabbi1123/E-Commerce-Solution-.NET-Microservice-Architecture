using Dapper;
using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IDbConnection _db;

        public DiscountRepository(IDbConnection db)
        {
            _db = db;
        }
        //public async Task<bool> CreateDiscount(ProductDiscount coupon)
        //{
        //    dbContext.Discount.Add(coupon);
        //    await dbContext.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> DeleteDiscount(int productId)
        //{
        //    var discount = await dbContext.Discount.FirstOrDefaultAsync(x => x.ProductId == productId);
        //    dbContext.Discount.Remove(discount);
        //    await dbContext.SaveChangesAsync();
        //    return true;
        //}

        public async Task<ProductDiscount?> GetDiscount(int productId)
        {
            const string sql = "SELECT * FROM discount WHERE ProductId = @productId";
            return await _db.QueryFirstOrDefaultAsync<ProductDiscount>(sql, new { productId });
        }

        //public async Task<bool> UpdateDiscount(ProductDiscount coupon)
        //{
        //    dbContext.Discount.Update(coupon);
        //    await dbContext.SaveChangesAsync();
        //    return true;
        //}
    }
}
