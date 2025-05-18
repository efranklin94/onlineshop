using MediatR;

namespace onlineshop.Commands.User.ToggleActivation;

public record ToggleActivationCommand(int Id) : IRequest;