using FluentValidation;

namespace Application.Catalog.Products.Commands.UpdateVariant;

public class UpdateVariantCommandValidator : AbstractValidator<UpdateVariantCommand>
{
    public UpdateVariantCommandValidator()
    {     
        RuleFor(x => x.RegularPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Regular price must be greater than or equal to 0.");
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");
    }
}
