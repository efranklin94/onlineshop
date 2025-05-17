using MediatR;
using onlineshop.Service;

namespace onlineshop.Commands.User.ToggleActivation;

public class ToggleActivationCommandHandler(IUserService service) : IRequestHandler<ToggleActivationCommand>
{
    public async Task Handle(ToggleActivationCommand request, CancellationToken cancellationToken)
    {
        await service.ToggleActivationAsync(request.Id, cancellationToken);
    }
}
