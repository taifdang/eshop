using Application.Common.Validation;
using FluentValidation;

namespace Application.Catalog.Products.Commands.CreateOptionValue;

public class CreateOptionValueCommandValidator : AbstractValidator<CreateOptionValueCommand>
{
    public CreateOptionValueCommandValidator()
    {
        RuleFor(x => x.OptionId).NotEmpty();
        RuleFor(x => x.Value).NotEmpty().WithMessage("Option value is required");

        When(x => x.MediaFile != null, () =>
        {
            RuleFor(x => x.MediaFile)
               .SetValidator(new FileValidator(1, [".jpg", ".jpeg", ".png"]));
        });
    }
}
