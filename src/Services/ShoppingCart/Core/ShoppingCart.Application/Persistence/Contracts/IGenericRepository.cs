namespace ShoppingCart.Application.Persistence.Contracts
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T> Get(int id);
		Task<List<T>> GetAll();
		Task<T> Add(T entity);
		Task Update(T entity);
		Task Delete(T entity);
	}
}
