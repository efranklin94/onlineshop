using FluentValidation;

namespace onlineshop.Commands.User.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.User.PhoneNumber)
            .NotNull().NotEmpty().WithMessage("PhoneNumber is required.")
            .MinimumLength(10).WithMessage("PhoneNumber length must be higher than 10.");
    }
}
