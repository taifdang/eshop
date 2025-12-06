using Application.Common.Validation;
using FluentValidation;

namespace Application.Catalog.Products.Commands.CreateProductImage;

public class CreateProductImageCommandValidator : AbstractValidator<CreateProductImageCommand>
{
    public CreateProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId).NotNull().WithMessage("ProductId is requried.");

        When(x => x.MediaFile != null, () =>
        {
            RuleFor(x => x.MediaFile)
               .SetValidator(new FileValidator(1, [".jpg", ".jpeg", ".png"]));
        });      
    }
}
