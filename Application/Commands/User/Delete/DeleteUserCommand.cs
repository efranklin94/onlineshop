using MediatR;

namespace onlineshop.Commands.User.Delete;

public record DeleteUserCommand(int Id) : IRequest;
