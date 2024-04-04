using MediatR;
using TaskProject.Domain.DTOs;

namespace TaskProject.Service.UseCases.Products.Commands
{
    public class UpdateProductCommand : UpdateProductDto, IRequest<int>
    {

    }
}