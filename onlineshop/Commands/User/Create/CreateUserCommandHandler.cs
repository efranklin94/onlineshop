using MediatR;
using onlineshop.DTOs;
using onlineshop.Exceptions;
using onlineshop.Service;

namespace onlineshop.Commands.User.Create;

public class CreateUserCommandHandler(IUserService service) : IRequestHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateUserCommandValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            throw new BadRequestException(string.Join(',', result.Errors));
        }

        await service.CreateAsync(request.User, cancellationToken);
    }
}
