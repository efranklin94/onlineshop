using MediatR;
using onlineshop.Data;
using onlineshop.Exceptions;
using onlineshop.Models;
using Resources;

namespace Application.Commands.UserTag.Create;

public class CreateUserTagCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserTagCommand>
{
    public async Task Handle(CreateUserTagCommand request, CancellationToken cancellationToken)
    {
        var User = await unitOfWork.UserRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(string.Format(Messages.NotFound, nameof(MyUser), request.Id));

        User.AddTag(request.Title, request.Priority);

        unitOfWork.UserRepository.Update(User);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}