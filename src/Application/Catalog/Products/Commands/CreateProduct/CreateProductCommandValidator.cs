using FluentValidation;

namespace Application.Catalog.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Title)
         .NotEmpty().WithMessage("Title is required.")
         .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
        RuleFor(x => x.Description)
         .NotEmpty().WithMessage("Description is required.")
         .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");
         
        //RuleFor(x => x.MinPrice)
        //    .GreaterThanOrEqualTo(0).WithMessage("Regular Price must be greater than or equal to 0.");
        //RuleFor(x => x.MaxPrice)
        //    .GreaterThanOrEqualTo(0).WithMessage("Compare Price must be greater than or equal to 0.")
        //    .GreaterThanOrEqualTo(x => x.MinPrice).WithMessage("Compare Price must be greater than or equal to regular Price.");
        //RuleFor(x => x.Quantity)
        //    .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0.");
    }
}
