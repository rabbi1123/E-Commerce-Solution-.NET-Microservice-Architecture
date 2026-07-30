using Discount.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly DiscountDbContext dbContext;

        public DiscountRepository(DiscountDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> CreateDiscount(ProductDiscount coupon)
        {
            dbContext.Discount.Add(coupon);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDiscount(int productId)
        {
            var discount = await dbContext.Discount.FirstOrDefaultAsync(x => x.ProductId == productId);
            dbContext.Discount.Remove(discount);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ProductDiscount> GetDiscount(int productId)
        {
            return await dbContext.Discount.FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<bool> UpdateDiscount(ProductDiscount coupon)
        {
            dbContext.Discount.Update(coupon);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
