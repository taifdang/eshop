using FluentValidation;

namespace Application.Catalog.Variants.Commands.UpdateVariant;

public class UpdateVariantCommandValidator : AbstractValidator<UpdateVariantCommand>
{
    public UpdateVariantCommandValidator()
    {     
        RuleFor(x => x.RegularPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Regular price must be greater than or equal to 0.");
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");
        RuleFor(x => x.Percent)
           .GreaterThanOrEqualTo(0).WithMessage("Percent must be greater than or equal to 0.");
        RuleFor(x => x.Sku)
       .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
    }
}
