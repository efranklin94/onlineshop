using onlineshop.Features;
using onlineshop.ViewModels;
using MediatR;

namespace onlineshop.Queries.User.GetUserList;

public record GetUserListQuery(
    string? Q,
    OrderType? OrderType,
    int? PageSize,
    int? PageNumber
    ) : IRequest<PaginationResult<UserViewModel>>;
