using FluentValidation;
using Resources;

namespace Application.Commands.UserTag.Create;

public class CreateUserTagCommandValidator : AbstractValidator<CreateUserTagCommand>
{
    public CreateUserTagCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotNull().NotEmpty()
            .WithMessage(string.Format(Messages.IsRequired, nameof(DomainModel.Models.UserTag.Title)));
    }
}
