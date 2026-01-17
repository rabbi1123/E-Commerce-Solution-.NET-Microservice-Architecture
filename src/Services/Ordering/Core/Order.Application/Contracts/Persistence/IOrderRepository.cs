using Order.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Contracts.Persistence
{
    public interface IOrderRepository : IAsyncRepository<ProductOrder>
    {
        Task<IEnumerable<ProductOrder>> GetOrdersByUserName(string userName);
    }
}
