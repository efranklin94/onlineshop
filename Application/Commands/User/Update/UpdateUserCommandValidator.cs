using FluentValidation;

namespace onlineshop.Commands.User.Update;


public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.User.PhoneNumber)
            .NotNull().NotEmpty().WithMessage("PhoneNumber is required.")
            .MinimumLength(10).WithMessage("PhoneNumber length must be higher than 10.");

        //RuleFor(x => x.User.Id)
        //    .GreaterThan(5);
    }
}
