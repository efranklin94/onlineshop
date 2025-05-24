using MediatR;
using onlineshop.Data;
using onlineshop.Exceptions;

namespace Application.Commands.UserTag.Delete;
public class DeleteUserTagCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserTagCommand>
{
    public async Task Handle(DeleteUserTagCommand request, CancellationToken cancellationToken)
    {
        var User = await unitOfWork.UserRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(string.Format(Messages.NotFound, nameof(DomainModel.Models.User), request.Id));

        User.RemoveTag(request.Title, request.Priority);

        unitOfWork.UserRepository.Update(User);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}