using Application.Commands.User.SetTrackingCodes;
using MediatR;

namespace Application.Jobs;

public class UsersTrackingCodeJob(IMediator mediator)
{
    public async Task Get()
    {
        var command = new SetTrackingCodesCommand();
        await mediator.Send(command);
    }
}
