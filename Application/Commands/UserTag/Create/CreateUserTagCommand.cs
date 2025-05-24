using MediatR;

namespace Application.Commands.UserTag.Create;

public record CreateUserTagCommand(int Id, string Title, int Priority) : IRequest;