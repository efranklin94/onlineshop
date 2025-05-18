using MediatR;
using onlineshop.Service;

namespace onlineshop.Commands.User.Delete;

public class DeleteUserCommandHandler(IUserService service) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(request.Id, cancellationToken);
    }
}
