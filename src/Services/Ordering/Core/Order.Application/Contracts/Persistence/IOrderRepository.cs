using Order.Domain.Entity;

namespace Order.Application.Contracts.Persistence
{
	public interface IOrderRepository : IAsyncRepository<ProductOrder>
	{
		Task<IEnumerable<ProductOrder>> GetOrdersByUserName(string userName);
	}
}
