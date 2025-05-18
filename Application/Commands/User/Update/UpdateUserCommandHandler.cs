using MediatR;
using onlineshop.Exceptions;
using onlineshop.Service;

namespace onlineshop.Commands.User.Update;

public class UpdateUserCommandHandler(IUserService service) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await service.UpdateAsync(request.id ,request.User, cancellationToken);
    }
}
