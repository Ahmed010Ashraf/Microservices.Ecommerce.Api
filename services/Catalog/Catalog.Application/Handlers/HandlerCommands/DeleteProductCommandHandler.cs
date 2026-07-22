using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Core.Reposatories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.HandlerCommands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {

        private readonly IProductReposatory _ProductRepo;

        public DeleteProductCommandHandler(IProductReposatory ProductRepo)
        {
            _ProductRepo = ProductRepo;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var result = await _ProductRepo.DeleteProduct(request.Id);
            return result;

        }
    }
}
