using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskProject.Domain.Entities;
using TaskProject.Service.Abstractions.DataAccess;
using TaskProject.Service.Abstractions.File;
using TaskProject.Service.UseCases.Products.Commands;

namespace TaskProject.Service.UseCases.Products.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;

        public DeleteProductCommandHandler(IApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
                return 0;

            await _fileService.DeletFileAsync(product.VideoPath);
            _context.Products.Remove(product);
            int response = await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}