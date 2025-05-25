using MediatR;
using onlineshop.Service;

namespace onlineshop.Commands.User.Create;

public class CreateUserCommandHandler(IUserService service) : IRequestHandler<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await service.CreateAsync(request.User, cancellationToken);
    }
}
