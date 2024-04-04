using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskProject.Domain.Entities;
using TaskProject.Service.Abstractions.DataAccess;
using TaskProject.Service.UseCases.Products.Queries;

namespace TaskProject.Service.UseCases.Products.Handlers
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllProductQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products.AsNoTracking().ToListAsync(cancellationToken);

            return products;
        }
    }
}