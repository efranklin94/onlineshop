﻿using MediatR;

namespace Application.Commands.BackOfficeUser.Login;

public record LoginCommand(string Username, string Password) : IRequest<string>;
