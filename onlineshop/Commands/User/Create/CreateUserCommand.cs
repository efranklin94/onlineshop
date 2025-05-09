using MediatR;
using onlineshop.DTOs;

namespace onlineshop.Commands.User.Create;

public record CreateUserCommand(CreateOrUpdateUserDTO User) : IRequest;
