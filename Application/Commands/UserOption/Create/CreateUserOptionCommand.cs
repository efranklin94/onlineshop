using MediatR;

namespace Application.Commands.UserOption.Create;

public record CreateUserOptionCommand(int Id, string Description) : IRequest;
