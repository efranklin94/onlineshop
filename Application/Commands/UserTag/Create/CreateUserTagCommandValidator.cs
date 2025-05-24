using FluentValidation;

namespace Application.Commands.UserTag.Create;

public class CreateUserTagCommandValidator : AbstractValidator<CreateUserTagCommand>
{
    public CreateUserTagCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotNull().NotEmpty()
            .WithMessage(string.Format(Messages.Required, nameof(DomainModel.Models.UserTag.Title)));
    }
}
