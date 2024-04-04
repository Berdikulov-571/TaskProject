using MediatR;
using System.Windows.Input;
using TaskProject.Domain.DTOs;

namespace TaskProject.Service.UseCases.Products.Commands
{
    public class CreateProductCommand : CreateProductDto, IRequest<int>
    {
    }
}