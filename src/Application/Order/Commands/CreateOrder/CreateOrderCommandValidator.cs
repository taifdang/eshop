using Domain.Enums;
using FluentValidation;

namespace Application.Order.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
         .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.Method)
         .IsInEnum().WithMessage("Invalid payment method.");

        RuleFor(x => x.Provider)
        .IsInEnum().WithMessage("PaymentProvider is invalid");
    }
}
