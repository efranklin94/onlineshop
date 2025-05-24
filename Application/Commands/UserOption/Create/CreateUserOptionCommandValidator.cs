using FluentValidation;

namespace Application.Commands.UserOption.Create;

public class CreateUserOptionCommandValidator : AbstractValidator<CreateUserOptionCommand>
{
    public CreateUserOptionCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotNull().NotEmpty()
            .WithMessage(string.Format(Messages.Required, nameof(DomainModel.Models.UserOption.Description)));
    }
}
