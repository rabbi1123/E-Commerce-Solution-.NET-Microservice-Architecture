using MediatR;
using Order.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.Orders.Queries.GetOrdersList
{
    public record GetOrderListQuery(string userName) : IRequest<List<OrderVm>>;
}
