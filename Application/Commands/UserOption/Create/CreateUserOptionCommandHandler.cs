using MediatR;
using onlineshop.Data;
using onlineshop.Exceptions;

namespace Application.Commands.UserOption.Create;

public class CreateUserOptionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserOptionCommand>
{
    public async Task Handle(CreateUserOptionCommand request, CancellationToken cancellationToken)
    {
        var car = await unitOfWork.UserRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(string.Format(Messages.NotFound, nameof(DomainModel.Models.MyUser), request.Id));

        car.AddOption(request.Description);

        unitOfWork.UserRepository.Update(car);
        await unitOfWork.CommitAsync(cancellationToken);
    }
}
