using MediatR;
using onlineshop.Data;
using onlineshop.Exceptions;
using onlineshop.Models;
using Resources;

namespace Application.Commands.UserOption.Delete;

public class DeleteUserOptionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserOptionCommand>
{
    public async Task Handle(DeleteUserOptionCommand request, CancellationToken cancellationToken)
    {
        var User = await unitOfWork.UserRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(string.Format(Messages.NotFound, nameof(MyUser), request.Id));

        User.RemoveOption(request.OptionId);

        unitOfWork.UserRepository.Update(User);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
