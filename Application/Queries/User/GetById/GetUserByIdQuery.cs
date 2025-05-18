using onlineshop.ViewModels;
using MediatR;

namespace onlineshop.Queries.User.GetById;

public record GetUserByIdQuery(int Id) : IRequest<UserViewModel>;
