using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(int Id, string UserName, decimal TotalPrice,
                                        string FirstName, string LastName, string EmailAddress,
                                        string AddressLine, string Country, string City,
                                        string ZipCode, string CardName, string CardNumber,
                                        string Expiration, string CVV,
                                        int PaymentMethod) : IRequest;
}
