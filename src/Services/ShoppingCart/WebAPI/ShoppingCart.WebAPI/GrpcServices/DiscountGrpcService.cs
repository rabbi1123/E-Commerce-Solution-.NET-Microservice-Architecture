using Discount.Grpc.Protos;

namespace ShoppingCart.WebAPI.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            this.discountProtoService = discountProtoService;
        }

        public async Task<DiscountModel> GetDiscount(int productId)
        {
            var discountRequest = new GetDiscountRequest { ProductId = productId };
            return await discountProtoService.GetDiscountAsync(discountRequest);
        }
    }
}
