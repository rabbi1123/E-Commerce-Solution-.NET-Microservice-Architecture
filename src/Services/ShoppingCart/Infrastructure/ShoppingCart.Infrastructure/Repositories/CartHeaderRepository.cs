using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Repositories
{
    public class CartHeaderRepository : GenericRepository<CartHeader>, ICartHeaderRepository
    {
        private readonly ShoppingCartDBContext dbContext;

        public CartHeaderRepository(ShoppingCartDBContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<CartHeader>> GetAllCartWithDetails()
        {
            var cartList = await dbContext.CartHeaders.Include(m => m.CartDetails).ToListAsync();
            return cartList;
        }

        public async Task<CartHeader> GetCartWithDetails(int id)
        {
            var cart = await dbContext.CartHeaders.Include(m => m.CartDetails).FirstOrDefaultAsync(x => x.Id == id);
            return cart;
        }
    }
}
