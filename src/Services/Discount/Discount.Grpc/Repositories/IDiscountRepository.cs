using Discount.Grpc.Models;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<ProductDiscount>? GetDiscount(int productId);
        //Task<bool> CreateDiscount(ProductDiscount coupon);
        //Task<bool> UpdateDiscount(ProductDiscount coupon);
        //Task<bool> DeleteDiscount(int productId);
    }
}
