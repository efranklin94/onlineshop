using onlineshop.Service;
using onlineshop.ViewModels;
using MediatR;

namespace onlineshop.Queries.User.GetById;

public class GetUserByIdQueryHandler(IUserService service) : IRequestHandler<GetUserByIdQuery, UserViewModel>
{
    public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await service.GetByIdAsync(request.Id, cancellationToken);
    }
}
