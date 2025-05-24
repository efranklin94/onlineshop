using MediatR;

namespace Application.Commands.UserTag.Delete;

public record DeleteUserTagCommand(int Id, string Title, int Priority) : IRequest;
