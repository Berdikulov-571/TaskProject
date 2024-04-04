using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskProject.Domain.Entities;
using TaskProject.Service.Abstractions.DataAccess;
using TaskProject.Service.UseCases.Products.Queries;

namespace TaskProject.Service.UseCases.Products.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IApplicationDbContext _context;

        public GetProductByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);

            return product;
        }
    }
}