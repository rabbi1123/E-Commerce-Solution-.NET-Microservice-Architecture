using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Application.Persistence.Contracts
{
	public interface ICartHeaderRepository : IGenericRepository<CartHeader>
	{
		Task<List<CartHeader>> GetAllCartWithDetails();
		Task<CartHeader> GetCartWithDetails(int id);
		Task<CartHeader> GetCartWithDetailsByUserId(string userId);
	}
}
