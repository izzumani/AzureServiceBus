using MediatR;
using Purchase.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.Behaviours
{
    public class AirtimePurchaseQueueingCommandBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : AirtimePurchaseQueueingCommand

    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Console.WriteLine("Airtime Purchase Queueing Command Behavior");
            return await next();
        }
    }
}
