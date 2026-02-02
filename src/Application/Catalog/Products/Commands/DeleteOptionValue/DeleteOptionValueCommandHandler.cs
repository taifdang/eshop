using Application.Abstractions;
using Application.Catalog.Products.Services;
using Application.Common.Interfaces;
using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Products.Commands.DeleteOptionValue
{
    public class DeleteOptionValueCommandHandler : IRequestHandler<DeleteOptionValueCommand, Unit>
    {
        private readonly IProductService _productService;
        private readonly IFileService _storageService;

        public DeleteOptionValueCommandHandler(
            IProductService productService, 
            IFileService storageService)
        {
            _productService = productService;
            _storageService = storageService;
        }

        public async Task<Unit> Handle(DeleteOptionValueCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(request.ProductId, cancellationToken);
            Guard.Against.NotFound(request.ProductId, product);

            var option = product.Options.FirstOrDefault(o => o.Id == request.OptionId);
            Guard.Against.NotFound(request.OptionId, option);

            var optionValue = option.Values.FirstOrDefault(v => v.Id == request.OptionValueId);
            Guard.Against.NotFound(request.OptionValueId, optionValue);

            // remove image from storage
            if (optionValue.Image != null)
            {
                await _storageService.DeleteFileAsync(new DeleteFileRequest { FileName = optionValue.Image.FileName });
            }

            product.RemoveOptionValue(request.OptionId, request.OptionValueId);

            await _productService.UpdateAsync(product, cancellationToken);

            return Unit.Value;
        }
    }
}
