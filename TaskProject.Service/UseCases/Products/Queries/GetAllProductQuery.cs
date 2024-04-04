using MediatR;
using TaskProject.Domain.Entities;

namespace TaskProject.Service.UseCases.Products.Queries
{
    public class GetAllProductQuery : IRequest<IEnumerable<Product>>
    {
    }
}