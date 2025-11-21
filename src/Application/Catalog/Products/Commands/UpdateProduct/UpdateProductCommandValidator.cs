using FluentValidation;

namespace Application.Catalog.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
        //RuleFor(x => x.Price)
        //    .GreaterThanOrEqualTo(0).WithMessage("Regular Price must be greater than or equal to 0.");
        //RuleFor(x => x.ComparePrice)
        //    .GreaterThanOrEqualTo(0).WithMessage("Compare Price must be greater than or equal to 0.")
        //    .GreaterThanOrEqualTo(x => x.Price).WithMessage("Compare Price must be greater than or equal to regular Price.");
        //RuleFor(x => x.Quantity)
        //    .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");
    }
}
