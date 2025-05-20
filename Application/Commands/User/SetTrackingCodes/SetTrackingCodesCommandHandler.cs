using DomainService.Specifications;
using MediatR;
using onlineshop.Data;
using onlineshop.Proxies;

namespace Application.Commands.User.SetTrackingCodes;

public class SetTrackingCodesCommandHandler(IUnitOfWork unitOfWork, ITrackingCodeProxy trackingCodeProxy) : IRequestHandler<SetTrackingCodesCommand>
{
    public async Task Handle(SetTrackingCodesCommand request, CancellationToken cancellationToken)
    {
        var specification = new GetUsersWithNoTrackinCodeSpecification();

        var (_, entities) = await unitOfWork.UserRepository.GetListAsync(specification, cancellationToken);

        if (entities.Count == 0)
        {
            return;
        }

        List<string> trackingCodes = new List<string>();

        try
        {
            trackingCodes = await trackingCodeProxy.Get(entities.Count, cancellationToken);

            if (trackingCodes.Count != entities.Count)
            {
                return;
            }
        }
        catch (Exception)
        {
            return;
        }

        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].SetTrackingCode(trackingCodes[i]);
        }

        try
        {
            unitOfWork.UserRepository.Update(entities);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            return;
        }
    }
}
