using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Behavior
{
    public class UnHandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();    
            }
            catch (Exception ex)
            {
                // Log the exception or perform any other necessary actions
                throw new Exception($"An unhandled exception occurred while processing the request of type {typeof(TRequest).Name}.", ex);
                throw;
            }
        }
    }
}
