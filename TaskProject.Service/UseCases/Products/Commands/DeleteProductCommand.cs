using MediatR;

namespace TaskProject.Service.UseCases.Products.Commands
{
    public class DeleteProductCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}