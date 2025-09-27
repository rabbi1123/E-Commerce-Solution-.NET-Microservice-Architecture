using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Product.Requests.Commands
{
    public record DeleteProductCommand(string Id) : IRequest;
}
