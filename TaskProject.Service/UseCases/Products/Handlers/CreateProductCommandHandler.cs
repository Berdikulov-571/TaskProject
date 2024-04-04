using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskProject.Domain.Entities;
using TaskProject.Service.Abstractions.DataAccess;
using TaskProject.Service.Abstractions.File;
using TaskProject.Service.UseCases.Products.Commands;

namespace TaskProject.Service.UseCases.Products.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;

        public CreateProductCommandHandler(IApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product? productResule = await _context.Products.FirstOrDefaultAsync(x => x.SortNumber == request.SortNumber,cancellationToken);

            if (productResule != null)
                return 0;

            Product product = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                SortNumber = request.SortNumber,
                VideoPath = await _fileService.UploadFileAsync(request.VideoPath),
            };

            await _context.Products.AddAsync(product,cancellationToken);
            int response = await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}