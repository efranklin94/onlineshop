using MediatR;

namespace Application.Commands.UserOption.Delete;

public record DeleteUserOptionCommand(int Id, Guid OptionId) : IRequest;
