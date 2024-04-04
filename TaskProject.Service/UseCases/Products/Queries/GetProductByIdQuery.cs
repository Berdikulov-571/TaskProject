using MediatR;
using TaskProject.Domain.Entities;

namespace TaskProject.Service.UseCases.Products.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
    }
}