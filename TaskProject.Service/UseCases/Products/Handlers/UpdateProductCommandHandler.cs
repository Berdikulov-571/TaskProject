using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskProject.Service.Abstractions.DataAccess;
using TaskProject.Service.Abstractions.File;
using TaskProject.Service.UseCases.Products.Commands;

namespace TaskProject.Service.UseCases.Products.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;

        public UpdateProductCommandHandler(IApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            var checkSortNumbere = await _context.Products.FirstOrDefaultAsync(x => x.SortNumber == request.SortNumber, cancellationToken);

            if (product == null || (product.SortNumber != checkSortNumbere.SortNumber && checkSortNumbere != null))
                return 0;

            if (request.Name != null)
            {
                product.Name = request.Name;
            }
            if(request.Description != null)
            {
                product.Description = request.Description;
            }
            if(request.SortNumber != null)
            {
                product.SortNumber = request.SortNumber;
            }
            if(request.VideoPath != null)
            {
                await _fileService.DeletFileAsync(product.VideoPath);
                product.VideoPath = await _fileService.UploadFileAsync(request.VideoPath);
            }

            _context.Products.Update(product);
            int response = await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}