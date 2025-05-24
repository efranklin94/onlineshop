using FluentValidation;
using Resources;

namespace Application.Commands.UserOption.Create;

public class CreateUserOptionCommandValidator : AbstractValidator<CreateUserOptionCommand>
{
    public CreateUserOptionCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotNull().NotEmpty()
            .WithMessage(string.Format(Messages.IsRequired, nameof(DomainModel.Models.UserOption.Description)));
    }
}
