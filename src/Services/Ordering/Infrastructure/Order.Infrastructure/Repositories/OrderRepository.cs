using Microsoft.EntityFrameworkCore;
using Order.Application.Contracts.Persistence;
using Order.Domain.Entity;
using Order.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<ProductOrder>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<ProductOrder>> GetOrdersByUserName(string userName)
        {
            var orderList = await dbContext.Orders
                                    .Where(o => o.UserName == userName)
                                    .ToListAsync();
            return orderList;
        }
    }
}
