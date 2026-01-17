using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.Orders.Commands.DeleteOrder
{
    public record DeleteOrderCommand(int Id) : IRequest;
}
