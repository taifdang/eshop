using Application.Common.Interfaces;
using Application.Common.Specifications;
using Application.Common.Validation;
using FluentValidation;

namespace Application.Catalog.Images.Commands.UploadFile;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    private readonly IRepository<Domain.Entities.ProductImage> _productImageRepository;
    public UploadFileCommandValidator(
        IRepository<Domain.Entities.ProductImage> productImageRepository,
        IRepository<Domain.Entities.Product> productRepository)
    {
        _productImageRepository = productImageRepository;

        RuleFor(x => x.MediaFile).SetValidator(new FileValidator(1, [".jpg", ".jpeg", ".png"]));

        RuleFor(x => x.ProductId)
          .MustAsync(ProductExists).WithMessage("Product not found.");

        RuleFor(x => x)
            .MustAsync(BeValidImageRules)
            .WithMessage("Invalid image rules.")
            .When(x => x.MediaFile != null);
    }

    private async Task<bool> ProductExists(int Id, CancellationToken ct)
    {
        var specification = new ProductFilterSpec(Id);
        await _productImageRepository.AnyAsync();
        return false;
    }


    private async Task<bool> BeValidImageRules(UploadFileCommand cmd, CancellationToken ct)
    {
        var specification = new ImageFilterSpec(cmd.ProductId, cmd.OptionValueId);

        if (cmd.OptionValueId is null)
        {  
            var imgs = await _productImageRepository.ListAsync(specification);
            var main = imgs.Count(x => x.IsMain);
            var subs = imgs.Count(x => !x.IsMain);
            return cmd.IsMain ? main < 1 : subs < 8;
        }
        // If image linked to option value, only one image allowed
        return !await _productImageRepository.AnyAsync(specification);
    }
}
