using MediatR;
using onlineshop.DTOs;

namespace onlineshop.Commands.User.Update;

public record UpdateUserCommand(int id, CreateOrUpdateUserDTO User) : IRequest;
