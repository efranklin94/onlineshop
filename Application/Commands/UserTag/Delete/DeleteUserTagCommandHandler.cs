using MediatR;
using onlineshop.Data;
using onlineshop.Exceptions;
using onlineshop.Models;
using Resources;

namespace Application.Commands.UserTag.Delete;
public class DeleteUserTagCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserTagCommand>
{
    public async Task Handle(DeleteUserTagCommand request, CancellationToken cancellationToken)
    {
        var User = await unitOfWork.UserRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(string.Format(Messages.NotFound, nameof(MyUser), request.Id));

        User.RemoveTag(request.Title, request.Priority);

        unitOfWork.UserRepository.Update(User);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}