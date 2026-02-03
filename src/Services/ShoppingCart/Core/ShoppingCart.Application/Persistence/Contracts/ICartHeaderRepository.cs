using ShoppingCart.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Persistence.Contracts
{
    public interface ICartHeaderRepository : IGenericRepository<CartHeader>
    {
        Task<List<CartHeader>> GetAllCartWithDetails();
        Task<CartHeader> GetCartWithDetails(int id);
		Task<CartHeader> GetCartWithDetailsByUserId(string userId);
	}
}
