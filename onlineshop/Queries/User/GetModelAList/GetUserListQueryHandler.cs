using onlineshop.Features;
using onlineshop.Service;
using onlineshop.ViewModels;
using MediatR;

namespace onlineshop.Queries.User.GetUserList;

public class GetUserListQueryHandler(IUserService service) : IRequestHandler<GetUserListQuery, PaginationResult<UserViewModel>>
{
    public async Task<PaginationResult<UserViewModel>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        return await service.GetListAsync(request.Q, request.OrderType, request.PageSize, request.PageNumber, cancellationToken);
    }
}
