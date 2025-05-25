using FluentValidation;
using Resources;

namespace Application.Commands.BackOfficeUser.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotNull().NotEmpty()
            .WithMessage(string.Format(Messages.IsRequired, nameof(DomainModel.Models.BackOfficeUser.Username)));

        RuleFor(x => x.Password)
            .NotNull().NotEmpty()
            .WithMessage(string.Format(Messages.IsRequired, nameof(DomainModel.Models.BackOfficeUser.Password)));
    }
}