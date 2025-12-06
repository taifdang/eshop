using FluentValidation;

namespace Application.Catalog.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
         .NotEmpty().WithMessage("Title is required.")
         .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
        RuleFor(x => x.Description)
         .NotEmpty().WithMessage("Description is required.")
         .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");         
    }
}
